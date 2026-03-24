using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    // button code for the play button on the main menu
    public void PlayButton()
    {
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