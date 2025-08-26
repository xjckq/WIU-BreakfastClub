using System.Collections.Generic;
using UnityEngine;

public class NPCPatrol : MonoBehaviour
{
    [SerializeField] List<Transform> waypoints = new List<Transform>();

    Vector3 destination;
    Vector3 direction;
    int index = 0;

    Rigidbody2D rb;
    [SerializeField] float speed = 2; 
    [SerializeField] float idleTime = 2;

    bool isWaiting;
    float waitTimer;
    public bool isInDialogue;
    Animator animator;
    SpriteRenderer spriteRenderer;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (waypoints.Count == 0) 
            return;

        destination = waypoints[index].position;
        direction = (destination - transform.position).normalized;
        rb.linearVelocity = direction * speed;

        isWaiting = false;
        isInDialogue = false;
        waitTimer = 0;
    }

    void FixedUpdate()
    {
        if (waypoints.Count == 0 || isInDialogue)
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("isMovingSide", false);
            animator.SetBool("isMovingUp", false);
            animator.SetBool("isMovingDown", false);
            return;
        }

        if (isWaiting)
        {
            rb.linearVelocity = Vector2.zero; 
            waitTimer += Time.fixedDeltaTime;

            animator.SetBool("isMovingSide", false);
            animator.SetBool("isMovingUp", false);
            animator.SetBool("isMovingDown", false);

            if (waitTimer >= idleTime)
            {
                isWaiting = false;
                index = (index + 1) % waypoints.Count;
                destination = waypoints[index].position;
                direction = (destination - transform.position).normalized;
            }
        }
        else
        {
            // move to destination
            rb.linearVelocity = direction * speed;

            animator.SetBool("isMovingSide", false);
            animator.SetBool("isMovingUp", false);
            animator.SetBool("isMovingDown", false);

            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
   
                animator.SetBool("isMovingSide", true);
                if (direction.x < 0)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                else
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
            }
            else
            {
                if (direction.y < 0)
                {
                    animator.SetBool("isMovingDown", true);
                }
                else if (direction.y > 0)
                {
                    animator.SetBool("isMovingUp", true);
                }
            }



            if (Vector3.Distance(transform.position, destination) < 0.1f)
            {
                isWaiting = true;
                waitTimer = 0;
            }
        }
    }
}
