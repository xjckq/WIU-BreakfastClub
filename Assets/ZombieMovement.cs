using UnityEngine;

public class ZombieMovement : MonoBehaviour
{
    public float moveSpeed = 1f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        rb.linearVelocity = Vector2.left * moveSpeed;
    }
}
