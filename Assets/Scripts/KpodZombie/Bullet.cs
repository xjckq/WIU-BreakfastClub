using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class Bullet : MonoBehaviour
{
    public float bulletLifetime = 3f;
    public float damage = 10f;
    public GameObject waterSplashEffectPrefab;
    public AudioClip waterSplashSound;
    private AudioSource audioSource;
    void Start()
    {
        Destroy(gameObject, bulletLifetime);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Bullet collided with: " + other.name);
        if (other.CompareTag("Zombie"))
        {
            GameObject splash = Instantiate(waterSplashEffectPrefab, other.transform.position, Quaternion.identity);
            ParticleSystem ps = splash.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                Destroy(splash, ps.main.duration);
                if (waterSplashSound != null)
                {
                    AudioSource.PlayClipAtPoint(waterSplashSound, transform.position);
                }
            }
            else
            {
                Destroy(splash, 2f);
            }
            Zombie zombie = other.GetComponent<Zombie>();
            if (zombie != null)
            {
                zombie.TakeDamage(damage);
            }
        }
        Destroy(gameObject);
    }
}
