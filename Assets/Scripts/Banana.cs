using UnityEngine;

public class Banana : MonoBehaviour
{
    private Rigidbody2D rb;
    public float launchForce = 5f;
    public int damageAmount = 10;
    [SerializeField] private float rotationSpeed = 50f;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }
    public void Launch(Vector2 direction)
    {
        if (rb != null)
        {
            rb.AddForce(direction.normalized * launchForce, ForceMode2D.Impulse);
        }
    }

    //void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("TopBoundingBox") ||
    //        collision.gameObject.CompareTag("BottomBoundingBox") || collision.gameObject.CompareTag("LeftBoundingBox") || collision.gameObject.CompareTag("RightBoundingBox")
    //         || collision.gameObject.CompareTag("Door"))
    //    {
    //        Destroy(gameObject);
    //    }
    //}
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController playerHealth = other.gameObject.GetComponent<PlayerController>();
            if (playerHealth != null)
            {
                playerHealth.TakeDmg(damageAmount);
            }
        }

        if (other.gameObject.CompareTag("Player") ||
            other.gameObject.CompareTag("TopBoundingBox") ||
            other.gameObject.CompareTag("BottomBoundingBox") ||
            other.gameObject.CompareTag("LeftBoundingBox") ||
            other.gameObject.CompareTag("RightBoundingBox") ||
            other.gameObject.CompareTag("Door"))
        {
            Destroy(gameObject);
        }
    }
}
