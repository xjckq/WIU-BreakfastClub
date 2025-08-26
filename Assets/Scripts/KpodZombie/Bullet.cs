using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletLifetime = 3f;
    public float damage = 10f;

    void Start()
    {
        Destroy(gameObject, bulletLifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Bullet collided with: " + other.name);
        if (other.CompareTag("Zombie"))
        {
            Zombie zombie = other.GetComponent<Zombie>();
            if (zombie != null)
            {
                zombie.TakeDamage(damage);
            }
        }
        Destroy(gameObject);
    }
}
