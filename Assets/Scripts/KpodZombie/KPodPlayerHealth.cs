using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class KPodPlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    [SerializeField] private int currentHealth;
    public Image HPbar;
    public WaveManager waveManager;
    void Awake()
    {
        currentHealth = maxHealth;
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Player took damage for " + damage + ". Current health: " + currentHealth);
        UpdateHealth();
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        waveManager.EndGame(false);
        gameObject.SetActive(false);
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        Debug.Log("Player healed for " + healAmount + ". Current health: " + currentHealth);
        UpdateHealth();
    }
    public void UpdateHealth()
    {
        if (HPbar != null)
            HPbar.fillAmount = (float)currentHealth / maxHealth;
    }
}
