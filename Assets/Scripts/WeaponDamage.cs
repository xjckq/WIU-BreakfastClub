using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    [SerializeField] private float damageAmount = 25f;
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealthSystem enemyHealth = other.GetComponent<EnemyHealthSystem>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damageAmount);
            }
        }
    }
}
