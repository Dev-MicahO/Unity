using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    public HashSet<string> destroyedObjects = new HashSet<string>();
    
    public bool GoldenBeagle = false;
    public bool SuspicousBrain = false;
    public bool RubyDagger = false;
    public bool KevlarVest = false;
    
    public static GameSession Instance;

    [Header("Battle State")]
    public bool isRandomEncounter = false;
    public bool tutorialBattleCompleted = false;

    [Header("Return To Overworld")]
    public Vector3 returnPlayerPosition = Vector3.zero;
    public bool hasReturnPosition = false;

    [Header("Persistent Player Combat Stats")]
    public int playerLevel = 1;
    public int playerXP = 0;
    public int xpToNextLevel = 100;

    public int playerMaxHP = 100;
    public int playerCurrentHP = 100;

    public bool hasPartyMember2 = false;
    public bool hasPartyMember3 = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Grabs stats from unit to save in gamesession.
    public void InitializePlayerStatsFromUnit(Unit playerUnit)
    {
        if (playerUnit == null)
            return;

        // Only initialize once, or whenever current HP is invalid
        if (playerMaxHP <= 0)
            playerMaxHP = playerUnit.maxHealth;

        if (playerCurrentHP <= 0 || playerCurrentHP > playerMaxHP)
            playerCurrentHP = playerMaxHP;
    }
    // Function to set the players hp when battle loads
    public void SetPlayerHP(int hp)
    {
        playerCurrentHP = Mathf.Clamp(hp, 0, playerMaxHP);
    }
    // Heal player after battle
    public void HealPlayer(int amount)
    {
        playerCurrentHP = Mathf.Clamp(playerCurrentHP + amount, 0, playerMaxHP);
    }

    // Here is how xp is added
    public void AddXP(int amount)
    {
        playerXP += amount;

        while (playerXP >= xpToNextLevel)
        {
            playerXP -= xpToNextLevel;
            LevelUp();
        }
    }

    // You leveled up congrats
    private void LevelUp()
    {
        playerLevel++;
        playerMaxHP += 10;
        playerCurrentHP = playerMaxHP;
        xpToNextLevel += 25;

        Debug.Log("Level up! Player is now level " + playerLevel);
    }

    public void setItemTrue(string itemName)
    {
        Debug.Log("setItemTrue called with: " + itemName);
        if(itemName == "Golden Beagle")
        {
            GoldenBeagle = true;
            Debug.Log("setItemTrue called with: " + itemName + " | Golden Beagle is: " + GoldenBeagle);
        }
        else if(itemName == "Suspicous Brain")
        {
            SuspicousBrain = true;
            Debug.Log("setItemTrue called with: " + itemName + " | Suspicous Brain is: " + SuspicousBrain);

        }
        else if(itemName == "Ruby Dagger")
        {
            RubyDagger = true;
            Debug.Log("setItemTrue called with: " + itemName + " | Ruby Dagger is: " + RubyDagger);

        }
        else if(itemName == "Kevlar Vest")
        {
            KevlarVest = true;
            playerMaxHP = playerMaxHP + 20;
            Debug.Log("setItemTrue called with: " + itemName + " | Kevlar Vest is: " + KevlarVest);

        }
    }

    public void setPartyMemberTrue(string partyMember)
    {
        Debug.Log("Party Member equals" + partyMember);
        if(partyMember == "Big Bam")
        {
            hasPartyMember2 = true;
        }
        else if(partyMember == "Old Man")
        {
            hasPartyMember3 = true;
        }

    }

    public bool getItemStatus(string itemName)
    {
        Debug.Log("getItemStatus called with: " + itemName);
        if(itemName == "Golden Beagle")
        {
            Debug.Log("getItemStatus called with: " + itemName + " | Golden Beagle is: " + GoldenBeagle);
            return GoldenBeagle;
        }
        else if(itemName == "Suspicous Brain")
        {
            Debug.Log("getItemStatus called with: " + itemName + " | Suspicous Brain is: " + SuspicousBrain);
            return SuspicousBrain;
        }
        else if(itemName == "Ruby Dagger")
        {
            Debug.Log("getItemStatus called with: " + itemName + " | Ruby Dagger is: " + RubyDagger);
            return RubyDagger;
        }
        else if(itemName == "Kevlar Vest")
        {
            Debug.Log("getItemStatus called with: " + itemName + " | Kevlar Vest is: " + KevlarVest);
            return KevlarVest;
        }
        
        return false;
        
    }

}