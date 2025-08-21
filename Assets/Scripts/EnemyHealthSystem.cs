using UnityEngine;
using UnityEngine.Events;

public class EnemyHealthSystem : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public bool isDead = false;
    [SerializeField] FloatingEnemyHealthBar healthBar;
    [SerializeField] private Vector3 healthBarOffset;
    [SerializeField] public UnityEvent OnDied;
    [SerializeField] private GameObject itemPrefab;
    private Vector3 initialScale;
    private NPCMovement _npcMovement;
    void Awake()
    {
        currentHealth = maxHealth;
        healthBar = GetComponentInChildren<FloatingEnemyHealthBar>();
        _npcMovement = GetComponent<NPCMovement>();

        initialScale = healthBar.transform.localScale;
    }
    private void Start()
    {
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
    }
    private void Update()
    {
        healthBar.transform.position = transform.position + healthBarOffset;

        healthBar.transform.localScale = new Vector3(
            Mathf.Abs(initialScale.x) * Mathf.Sign(transform.localScale.x),
            initialScale.y,
            initialScale.z
        );
        if (Input.GetKeyDown(KeyCode.Y))
        {
            TakeDamage(20);
            Debug.Log(currentHealth);
        }
    }
    public void TakeDamage(float damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
        Debug.Log(gameObject.name + " took " + damageAmount + " damage. Current health: " + currentHealth);

        if (currentHealth < maxHealth)
        {
            _npcMovement.OnTakeDamage();
            Debug.Log("call change state function on take dmg");
        }
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        if (itemPrefab != null)
        {
            Instantiate(itemPrefab, transform.position, Quaternion.identity);
        }
        Debug.Log(gameObject.name + " has died!");
        OnDied.Invoke();
        Destroy(gameObject);
    }
}
