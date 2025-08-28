using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Zombie : MonoBehaviour
{
    public ZombieData zombieData;
    private float currentHealth;
    private float maxHealth;
    private float currentMoveSpeed;
    private Rigidbody2D rb;
    private WaveManager waveManager;
    public Image healthBarFill;
    public Image healthBarBorder;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    public float flashDuration = 0.1f;
    public Color flashColor = Color.red;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = zombieData.health;
        maxHealth = zombieData.health;
        currentMoveSpeed = zombieData.moveSpeed;
        waveManager = FindAnyObjectByType<WaveManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }
    void FixedUpdate()
    {
        Vector2 movement = Vector2.left * currentMoveSpeed;
        rb.linearVelocity = movement;
    }
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        if (healthBarFill != null)
        {
            float healthRatio = currentHealth / maxHealth;
            healthBarFill.fillAmount = healthRatio;
        }
        StartCoroutine(FlashEffect());
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        Debug.Log(gameObject.name + " has died!");
        if (waveManager != null)
        {
            waveManager.EnemyKilled();
        }
        Destroy(gameObject);
    }
    private IEnumerator FlashEffect()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.color = originalColor;
        }
    }
    void Move()
    {
        transform.Translate(Vector2.left * currentMoveSpeed * Time.deltaTime);
    }
}
