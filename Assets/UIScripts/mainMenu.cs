using UnityEngine;
using UnityEngine.InputSystem;

public class mainMenu : MonoBehaviour
{
    // main menu ui panel
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;

    // player controls for key inputs
    private PlayerControls controls;

    // allows you to use key presses to close certain menus
    void Awake()
    {
        controls = new PlayerControls();

        controls.MainMenuUI.closeSettings.performed += ctx =>
        {
            if (settingsPanel.activeSelf)
            {
                CloseSettings();
            }
        };
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    // changes to the settings panel
    public void OpenSettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    // changes to the main menu panel 
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}