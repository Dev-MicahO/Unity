using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{   
    // -- Micah's code:
    public GameObject mainMenuPanel;
    public GameObject introPanel;
    public GameObject classPanel;
    
    // button code for the play button on the main menu
    public void PlayButton()
    {
        if (mainMenuPanel != null)
        mainMenuPanel.SetActive(false);

        if (introPanel != null)
        introPanel.SetActive(true);

        if (classPanel != null)
        classPanel.SetActive(false);  
    }

    // Continue from the intro panel to the class selection panel
    public void ContinueToClassSelect()
    {
        if (introPanel != null)
        introPanel.SetActive(false);

        if (classPanel != null)
        classPanel.SetActive(true);
    }

    // Go back from intro panel to main menu
    public void BackToMainMenuFromIntro()
    {
        if (introPanel != null)
        introPanel.SetActive(false);

        if (mainMenuPanel != null)
        mainMenuPanel.SetActive(true);

        if (classPanel != null)
        classPanel.SetActive(false);
    }

    // Go back from class selection panel to intro panel
    public void BackToIntroFromClassSelect()
    {
        if (classPanel != null)
        classPanel.SetActive(false);

        if (introPanel != null)
        introPanel.SetActive(true);

        if (mainMenuPanel != null)
        mainMenuPanel.SetActive(false);
    }
    
    // Class picking 
    public void SelectWarrior()
    {
        SelectClassAndStart(PlayerClass.Warrior);
    }

    public void SelectMage()
    {
        SelectClassAndStart(PlayerClass.Mage);
    }

    public void SelectDoctor()
    {
        SelectClassAndStart(PlayerClass.Doctor);
    }
    public void SelectThief()
    {
        SelectClassAndStart(PlayerClass.Thief);
    }
    
    // Method to pick the class and start the game
    private void SelectClassAndStart(PlayerClass chosenClass)
    {
        if (GameSession.Instance != null)
        {
            GameSession.Instance.SetClass(chosenClass);
            GameSession.Instance.isRandomEncounter = false;
        }

        SceneChanger.Instance.LoadScene("Battlescene");
    }

    // button code for the quit button on the main menu
    public void QuitButton()
    {
        Debug.Log("exited");
        Application.Quit();
    }

    // button code for the back button on the settings menu 
    public void BackButton()
    {
        SceneChanger.Instance.LoadScene("main menu");
    }

    // button code for the quit to desktop button in the pause menu
    public void QuitToDesktop()
    {
        PlayerPrefs.Save();

        Debug.Log("Game Saved!");

        Application.Quit();
        
    }

    // button code for the quit to title button in the pause menu 
    public void QuitToMenu()
    {
        Time.timeScale = 1f;
        PlayerPrefs.Save();

        Debug.Log("Game Saved!");

        SceneChanger.Instance.LoadScene("main menu");
    }
}