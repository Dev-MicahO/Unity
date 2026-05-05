using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XPBarUI : MonoBehaviour
{
    [Header("XP Bar")]
    [SerializeField] private Image xpFillImage;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI xpText;

    private void OnEnable()
    {
        UpdateXPBar();
    }

    private void Update()
    {
        UpdateXPBar();
    }

    public void UpdateXPBar()
    {
        if (xpFillImage == null)
        {
            Debug.LogWarning("XPBarUI: xpFillImage is not assigned.");
            return;
        }

        if (GameSession.Instance == null)
        {
            Debug.LogWarning("XPBarUI: GameSession.Instance is null.");
            return;
        }

        int currentLevel = GameSession.Instance.playerLevel;
        int currentXP = GameSession.Instance.playerXP;
        int maxLevel = GameSession.Instance.maxLevel;

        if (currentLevel >= maxLevel)
        {
            xpFillImage.fillAmount = 1f;

            if (levelText != null)
                levelText.text = "LVL " + currentLevel + ":";

            if (xpText != null)
                xpText.text = "MAX LEVEL";

            return;
        }

        int previousLevelXP = GameSession.Instance.GetXPRequiredForCurrentLevel();
        int nextLevelXP = GameSession.Instance.GetXPRequiredForNextLevel();

        int xpIntoLevel = currentXP - previousLevelXP;
        int xpNeededThisLevel = nextLevelXP - previousLevelXP;

        float xpPercent = 0f;

        if (xpNeededThisLevel > 0)
        {
            xpPercent = Mathf.Clamp01((float)xpIntoLevel / xpNeededThisLevel);
        }

        xpFillImage.fillAmount = xpPercent;

        if (levelText != null)
            levelText.text = "LVL " + currentLevel + ":";

        if (xpText != null)
            xpText.text = "XP: " + xpIntoLevel + "/" + xpNeededThisLevel;
    }
}