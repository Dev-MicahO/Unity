using UnityEngine;

public class FinalFightTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other) 
    {
        Debug.Log(other.name);
        //StartScriptedBossFight();

    }
}
