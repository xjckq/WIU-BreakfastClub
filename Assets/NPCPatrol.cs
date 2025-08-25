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
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
        waitTimer = 0;
    }

    void FixedUpdate()
    {
        if (waypoints.Count == 0) 
            return;

        if (isWaiting)
        {
            rb.linearVelocity = Vector2.zero; 
            waitTimer += Time.fixedDeltaTime;

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
            if (Vector3.Distance(transform.position, destination) < 0.1f)
            {
                isWaiting = true;
                waitTimer = 0;
            }
        }
    }
}
