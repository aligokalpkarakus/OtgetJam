using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public int damageAmount = 10; // Default 10 damage (b�y�k bal�k i�in)

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

            //Destroy(gameObject); // Z�pk�n veya a� yok olsun
        }
    }

}
