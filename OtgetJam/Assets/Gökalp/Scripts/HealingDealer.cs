using UnityEngine;

public class HealingDealer : MonoBehaviour
{
    public int healAmount = 5; // Küçük balýktan gelen can miktarý

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HealthSystem health = collision.GetComponent<HealthSystem>();
            if (health != null)
            {
                SoundManager.Instance.PlaySound(0);
                health.Heal(healAmount);
                Destroy(gameObject); // Küçük balýk yenir
            }
        }
    }
}
