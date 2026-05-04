using UnityEngine;

public class SaveMenuButtons : MonoBehaviour
{
    public void SaveGame()
    {
        if (SaveManager.Instance != null)
            SaveManager.Instance.SaveGame();
        else
            Debug.LogWarning("No SaveManager instance found.");
    }

    public void LoadGame()
    {
        if (SaveManager.Instance != null)
            SaveManager.Instance.LoadGameFromMenu();
        else
            Debug.LogWarning("No SaveManager instance found.");
    }

    public void DeleteSave()
    {
        if (SaveManager.Instance != null)
            SaveManager.Instance.DeleteSaveData();
        else
            Debug.LogWarning("No SaveManager instance found.");
    }
}