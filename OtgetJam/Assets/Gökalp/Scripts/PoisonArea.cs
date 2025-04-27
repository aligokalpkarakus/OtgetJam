using UnityEngine;

public class PoisonArea : MonoBehaviour
{
    [Header("Poison Settings")]
    [SerializeField] private int damagePerSecond = 1;
    [SerializeField] private float tickRate = 0.25f; // Daha smooth effect için

    private bool isPlayerInside = false;
    private HealthSystem playerHealth;
    private float damageTimer = 0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SoundManager.Instance.PlaySound(4);
            playerHealth = collision.GetComponent<HealthSystem>();
            isPlayerInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInside = false;
            playerHealth = null;
        }
    }

    private void Update()
    {
        if (isPlayerInside && playerHealth != null)
        {
            damageTimer += Time.deltaTime;

            // tickRate süresine göre hasar ver
            if (damageTimer >= tickRate)
            {
                // tickRate süresine göre hasar miktarýný hesapla
                float damageAmount = damagePerSecond * tickRate;
                playerHealth.TakeDamage(Mathf.CeilToInt(damageAmount)); // Yukarý yuvarlama
                damageTimer = 0f;
            }
        }
    }
}