using UnityEngine;

public class HealPickup : MonoBehaviour
{
    [SerializeField] private int healAmount = 25;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (GameSession.Instance == null)
            return;

        if (GameSession.Instance.playerCurrentHP < GameSession.Instance.playerMaxHP)
        {
            GameSession.Instance.HealPlayer(healAmount);
            Debug.Log("Health pack collected. HP is now " + GameSession.Instance.playerCurrentHP);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Player is already at full HP.");
        }
    }
}