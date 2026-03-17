using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{

    public BattleState state;

    // Units
    public Unit playerUnit;
    public Unit enemyUnit;
    public Unit zombieUnit;
    public Unit bossUnit;

    // Battle Text
    public TextMeshProUGUI battleText;
    public TextMeshProUGUI playerHPText;
    public TextMeshProUGUI enemyHPText;

    // HP Bars
    public Image playerHPBarFill;
    public Image enemyHPBarFill;
    private float playerTargetHPFill;
    private float enemyTargetHPFill;
    public float hpBarSpeed = 2f;

    // Action Buttons
    public Button attackButton;
    public Button defendButton;
    public Button skillButton;
    public Button fleeButton;

    // Panels / UI
    public GameObject damagePopupPrefab;
    public GameObject actionPanel;
    public GameObject skillPanel;
    public GameObject zombieObject;
    public GameObject bossObject;

    // Damage Popup Points
    public Transform playerDamagePoint;
    public Transform enemyDamagePoint;

    // Canvas
    public Canvas battleCanvas;

    // Skills
    public int powerStrikeDamage = 35;

    // Boss Fight
    private bool bossFightStarted = false;

    //Effects
    [Header("Hit Feedback")]
    // How long a target shakes when hit
    public float shakeDuration = 0.2f;
    // How much the target moves while shaking
    public float shakeMagnitude = 0.05f;
    // How long the target flashes when hit

    public float flashDuration = 0.15f;
    // The color the target flashes when hit (light red)
    public Color flashColor = new Color(1f, 0.7f, 0.7f, 1f);

    void Start()
    {
        Debug.Log("BattleManager started");

        // Start both HP bars at full 
        playerTargetHPFill = 1f;
        enemyTargetHPFill = 1f;

        // Set the first enemy to the zombie
        enemyUnit = zombieUnit;

        // Show zombie at start, hide boss until needed
        zombieObject.SetActive(true);
        bossObject.SetActive(false);

        // Shows the main action panel and disables buttons until setup is done
        ShowActionPanel();
        SetActionButtonsInteractable(false);

        // Start the battle setup sequence
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    // Spawns a floating damage number at a given world position
    void ShowDamagePopup(Transform damagePoint, int damage)
    {
        // Convert the world position to a screen position for UI placement
        Vector3 screenPos = Camera.main.WorldToScreenPoint(damagePoint.position);
        
        // Create the popup under the battle canvas
        GameObject popupObj = Instantiate(damagePopupPrefab, battleCanvas.transform);

        // Place the popup on screen
        RectTransform popupRect = popupObj.GetComponent<RectTransform>();
        popupRect.position = screenPos;
       
        // Pass the damage value into the popup script
        DamagePopup popup = popupObj.GetComponent<DamagePopup>();
        popup.Setup(damage);
    }
   
    // Makes a target briefly shake when hit    
    IEnumerator ShakeTarget(Transform target)
    {
        Vector3 originalPosition = target.localPosition;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float offsetX = Random.Range(-shakeMagnitude, shakeMagnitude);
            float offsetY = Random.Range(-shakeMagnitude, shakeMagnitude);

            target.localPosition = originalPosition + new Vector3(offsetX, offsetY, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }
        // Restore original position after shaking
        target.localPosition = originalPosition;
    }
    
    // Makes a sprite briefly flash a different color when hit
    IEnumerator FlashTarget(SpriteRenderer spriteRenderer)
    {
        if (spriteRenderer == null)
            yield break;

        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = flashColor;

        yield return new WaitForSeconds(flashDuration);

        spriteRenderer.color = originalColor;
    }
    
    // Gets the SpriteRenderer from a Unit object
    SpriteRenderer GetUnitSpriteRenderer(Unit unit)
    {
        return unit.GetComponent<SpriteRenderer>();
    }
    
    /* Instantly updates both HP text and HP bar fill amounts
       Used at battle start and during boss transition
    */
    void RefreshHPUIImmediate()
    {
        UpdateHPText();

        playerHPBarFill.fillAmount = playerTargetHPFill;
        enemyHPBarFill.fillAmount = enemyTargetHPFill;
    }
   
    // Shows the normal action panel and hides the skill panel
    void ShowActionPanel()
    {
        actionPanel.SetActive(true);
        skillPanel.SetActive(false);
    }
    
    // Shows the skill panel and hides the normal action panel
    void ShowSkillPanel()
    {
        actionPanel.SetActive(false);
        skillPanel.SetActive(true);
    }
    
    // Updates HP text and sets the target fill values for the HP bars
    void UpdateHPText()
    {
        playerHPText.text = playerUnit.unitName + " HP: " + playerUnit.currentHealth + "/" + playerUnit.maxHealth;
        enemyHPText.text = enemyUnit.unitName + " HP: " + enemyUnit.currentHealth + "/" + enemyUnit.maxHealth;

        playerTargetHPFill = (float)playerUnit.currentHealth / playerUnit.maxHealth;
        enemyTargetHPFill = (float)enemyUnit.currentHealth / enemyUnit.maxHealth;
    }

    // Starts the second phase of battle by swapping from zombie to boss
    IEnumerator StartBossFight()
    {
        state = BattleState.BUSY;
        SetActionButtonsInteractable(false);

        battleText.text = enemyUnit.unitName + " was defeated!";
        yield return new WaitForSeconds(1.5f);

        battleText.text = "A stronger enemy approaches!";
        yield return new WaitForSeconds(1.5f);
      
        // Swap enemy from zombie to boss
        zombieObject.SetActive(false);
        bossObject.SetActive(true);

        //Swap the current enemy reference to the boss unit
        enemyUnit = bossUnit;
        bossFightStarted = true;

        //Refresh enemy HP UI for the boss
        RefreshHPUIImmediate();

        battleText.text = enemyUnit.unitName + " enters the battle!";
        yield return new WaitForSeconds(1.5f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }
    
    // Handles the opening setup for the battle
    IEnumerator SetupBattle()
    {
        SetActionButtonsInteractable(false);

        battleText.text = "A zombie appears!";
        UpdateHPText();
        RefreshHPUIImmediate();

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }
    
    // Starts the player's turn
    void PlayerTurn()
    {
        battleText.text = "Choose an action.";
        SetActionButtonsInteractable(true);
        ShowActionPanel();
    }
    
    // Enables or disables the main action buttons
    void SetActionButtonsInteractable(bool isInteractable)
    {
        attackButton.interactable = isInteractable;
        defendButton.interactable = isInteractable;
        skillButton.interactable = isInteractable;
        fleeButton.interactable = isInteractable;
    }
    
    // Called when the Attack button is pressed
    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        SetActionButtonsInteractable(false);
        StartCoroutine(PlayerAttack());
    }

    // Called when the Defend button is pressed
    public void OnDefendButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        SetActionButtonsInteractable(false);
        StartCoroutine(PlayerDefend());
    }
    // Called when the Skill button is pressed
    public void OnSkillButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        ShowSkillPanel();
        battleText.text = "Choose a skill.";
    }
    // Called when the Back button on the skill panel is pressed
    public void OnBackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        ShowActionPanel();
        battleText.text = "Choose an action.";
    }
    
    // Called when the Flee button is pressed
    public void OnFleeButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerFlee());
    }
    
    // Called when the Power Strike skill button is pressed
    public void OnPowerStrikeButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerPowerStrike());
    }
    
    // Handles the player's normal attack
    IEnumerator PlayerAttack()
    {
        state = BattleState.BUSY;

        yield return new WaitForSeconds(0.5f);

        int damage = playerUnit.GetDamage();

        enemyUnit.TakeDamage(damage);
        StartCoroutine(ShakeTarget(enemyUnit.transform));
        StartCoroutine(FlashTarget(GetUnitSpriteRenderer(enemyUnit)));
        ShowDamagePopup(enemyDamagePoint, damage);
        UpdateHPText();

        battleText.text = "You attacked for " + damage + " damage!";

        yield return new WaitForSeconds(1.5f);
         
        // If zombie dies first, begin boss phase
        if (enemyUnit.IsDead())
        {
            if (!bossFightStarted)
            {
                StartCoroutine(StartBossFight());
            }
            else
            {
            // If boss dies, player wins ( you will not win >:) )
                state = BattleState.WON;
                EndBattle();
            }
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }
    
    // Handles defending for one enemy attack
    IEnumerator PlayerDefend()
    {
        state = BattleState.BUSY;
        SetActionButtonsInteractable(false);

        playerUnit.isDefending = true;
        battleText.text = "You brace for impact!";

        yield return new WaitForSeconds(1f);

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }
    
    // Handles fleeing from battle
    IEnumerator PlayerFlee()
    {
        state = BattleState.BUSY;
        SetActionButtonsInteractable(false);

        battleText.text = "You fled from battle!";

        yield return new WaitForSeconds(1f);

        state = BattleState.FLED;
        EndBattle();
    }

    // Handles the Power Strike skill
    IEnumerator PlayerPowerStrike()
    {
        state = BattleState.BUSY;
        SetActionButtonsInteractable(false);
        ShowActionPanel();

        int psdamage = Random.Range(powerStrikeDamage - 5, powerStrikeDamage + 6);

        battleText.text = "You use Power Strike!";

        yield return new WaitForSeconds(0.5f);

        enemyUnit.TakeDamage(psdamage);
        StartCoroutine(ShakeTarget(enemyUnit.transform));
        StartCoroutine(FlashTarget(GetUnitSpriteRenderer(enemyUnit)));
        ShowDamagePopup(enemyDamagePoint, psdamage);
        UpdateHPText();

        battleText.text = "Power Strike deals " + psdamage + " damage!";

        yield return new WaitForSeconds(1.5f);

        if (enemyUnit.IsDead())
        {
            if (!bossFightStarted)
            {
                StartCoroutine(StartBossFight());
            }
            else
            {
                state = BattleState.WON;
                EndBattle();
            }
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }
    
    // Handles the enemy's turn
    IEnumerator EnemyTurn()
    {
        state = BattleState.BUSY;

        battleText.text = "Enemy attacks!";

        yield return new WaitForSeconds(0.5f);

        int damage = enemyUnit.GetDamage();
        bool blocked = false;
        
        // If the player defended, reduce the damage
        if (playerUnit.isDefending)
        {
            damage = Mathf.Max(1, damage / 2);
            playerUnit.isDefending = false;
            blocked = true;
        }

        playerUnit.TakeDamage(damage);
        StartCoroutine(ShakeTarget(playerUnit.transform));
        StartCoroutine(FlashTarget(GetUnitSpriteRenderer(playerUnit)));
        ShowDamagePopup(playerDamagePoint, damage);
        UpdateHPText();

        if (blocked)
            battleText.text = "You blocked part of the damage! Enemy dealt " + damage + " damage!";
        else
            battleText.text = "Enemy attacks for " + damage + " damage!";

        yield return new WaitForSeconds(1.5f);

        if (playerUnit.IsDead())
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }
    
    // Smoothly animates HP bars every frame toward their target values
    void Update()
    {
        playerHPBarFill.fillAmount = Mathf.Lerp(
            playerHPBarFill.fillAmount,
            playerTargetHPFill,
            Time.deltaTime * hpBarSpeed
        );
        enemyHPBarFill.fillAmount = Mathf.Lerp(
            enemyHPBarFill.fillAmount,
            enemyTargetHPFill,
            Time.deltaTime * hpBarSpeed
        );
    }

    // Displays the correct final battle message and disables player input
    void EndBattle()
    {
        SetActionButtonsInteractable(false);

        if (state == BattleState.WON)
            battleText.text = "You won!";
        else if (state == BattleState.LOST)
            battleText.text = "You were defeated!";
        else if (state == BattleState.FLED)
            battleText.text = "You fled from battle!";
    }
}
