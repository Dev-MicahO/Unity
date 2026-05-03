using UnityEngine;

public class EncounterAreaSetter : MonoBehaviour
{
    public string encounterArea = "Forest";
    void Start()
    {
        if (GameSession.Instance != null)
        {
            GameSession.Instance.currentEncounterArea = encounterArea;
            Debug.Log("Encounter Area set to: " + encounterArea);
        }
    }
}

