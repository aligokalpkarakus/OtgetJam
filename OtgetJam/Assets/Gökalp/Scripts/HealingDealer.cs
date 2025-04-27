using UnityEngine;

public class HealingDealer : MonoBehaviour
{
    public int healAmount = 5; // K���k bal�ktan gelen can miktar�

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HealthSystem health = collision.GetComponent<HealthSystem>();
            if (health != null)
            {
                SoundManager.Instance.PlaySound(0);
                health.Heal(healAmount);
                Destroy(gameObject); // K���k bal�k yenir
            }
        }
    }
}
