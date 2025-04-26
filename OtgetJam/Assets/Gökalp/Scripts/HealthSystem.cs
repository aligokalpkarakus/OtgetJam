using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement; // SceneManager i�in gerekli

public class HealthSystem : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Health Bar Settings")]
    [SerializeField] private SpriteRenderer healthBarTop; // �stteki bal�k sprite'�
    private Material healthMaterial;
    private float initialCanValue; // "Can" de�eri

    private void Start()
    {
        currentHealth = maxHealth;

        if (healthBarTop != null)
        {
            healthMaterial = healthBarTop.material;
            initialCanValue = healthMaterial.GetFloat("_Can"); // Materyaldeki "Can" property
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();
    }

    public void TakeDamageOverTime(int damagePerSecond, float duration)
    {
        StartCoroutine(DamageOverTimeCoroutine(damagePerSecond, duration));
    }

    private IEnumerator DamageOverTimeCoroutine(int dps, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            TakeDamage(dps);
            yield return new WaitForSeconds(1f);
            timer += 1f;
        }
    }

    private void UpdateHealthBar()
    {
        if (healthMaterial != null)
        {
            // Sa�l�k oran�na g�re "Can" property'sini g�ncelle
            float newCanValue = Mathf.Lerp(0f, initialCanValue, (float)currentHealth / maxHealth);
            healthMaterial.SetFloat("_Can", newCanValue);
        }
    }

    private void Die()
    {
        Debug.Log("Player died! Restarting scene...");

        // Sahneyi yeniden y�kle
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);


    }

  
}