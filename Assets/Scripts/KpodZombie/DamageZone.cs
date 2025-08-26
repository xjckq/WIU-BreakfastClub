using UnityEngine;

public class DamageZone : MonoBehaviour
{
    public KPodPlayerHealth kpodPlayerHealth;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Zombie zombie = other.GetComponent<Zombie>();
        if (zombie != null)
        {
            if (kpodPlayerHealth != null)
            {
                int zombieDamage = zombie.zombieData.damage;
                kpodPlayerHealth.TakeDamage(zombieDamage);
            }
            zombie.Die();
        }
    }
}
