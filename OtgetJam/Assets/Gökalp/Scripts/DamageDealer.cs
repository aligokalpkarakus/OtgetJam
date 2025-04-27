using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public int damageAmount = 10; // Default 10 damage (büyük balýk için)

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HealthSystem health = collision.GetComponent<HealthSystem>();
            if (health != null)
            {
                SoundManager.Instance.PlaySound(0);
                health.TakeDamage(damageAmount);
            }

            //Destroy(gameObject); // Zýpkýn veya að yok olsun
        }
    }

}
