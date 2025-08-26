using UnityEngine;

public class Zombie : MonoBehaviour
{
    public ZombieData zombieData;
    private float currentHealth;
    private float currentMoveSpeed;
    private Rigidbody2D rb;
    private WaveManager waveManager;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = zombieData.health;
        currentMoveSpeed = zombieData.moveSpeed;
        waveManager = FindAnyObjectByType<WaveManager>();
    }
    void FixedUpdate()
    {
        Vector2 movement = Vector2.left * currentMoveSpeed;
        rb.linearVelocity = movement;
    }
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        //Debug.Log(gameObject.name + " took " + damageAmount + " damage! Current health: " + currentHealth);
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

    void Move()
    {
        transform.Translate(Vector2.left * currentMoveSpeed * Time.deltaTime);
    }
}
