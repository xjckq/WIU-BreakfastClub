using UnityEngine;

public class Chicken : MonoBehaviour
{
    public float speed = 2;
    public float stopDistance = 1.5f;
    Vector2 playerPos, chickenPos, newPos, playerLastPos;

    [SerializeField] GameObject player;

    public float pushDetectionRadius = 1.5f;
    public float pushDistance = 1.2f;
    public float pushSpeed = 4;
    public float pushDuration = .8f;
    public float pushRandomness = .3f;

    float pushTimer = 0;
    Vector2 pushTargetPos;
    float distanceToPlayer;

    Rigidbody2D playerRB;

    void Start()
    {
        playerRB = player.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        chickenPos = transform.position;
        playerPos = player.transform.position;

        pushTimer -= Time.deltaTime;

        if (pushTimer > 0f)
        {
            // move toward push target
            newPos = Vector2.MoveTowards(chickenPos, pushTargetPos, pushSpeed * Time.deltaTime);
            transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
            return;
        }


        distanceToPlayer = Vector2.Distance(chickenPos, playerPos);

        // check for push behavior
        if (distanceToPlayer < pushDetectionRadius && IsPlayerMovingTowardsChicken())
        {
            Vector2 pushDirection = (chickenPos - playerPos).normalized;

            // set push target position
            pushTargetPos = chickenPos + pushDirection * pushDistance;
            pushTimer = pushDuration;

            // face away from player
            if (pushDirection.x > 0)
                transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
            else
                transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);

            return;
        }

        // follow player
        if (distanceToPlayer > stopDistance)
        {
            newPos = Vector2.MoveTowards(chickenPos, playerPos, speed * Time.deltaTime);
            transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
        }
    }

    bool IsPlayerMovingTowardsChicken()
    {
        // check if player is moving fast enough
        if (playerRB.linearVelocity.magnitude < 0.5f)
            return false;

        // check if player is getting closer to chicken
        float lastDistance = Vector2.Distance(playerLastPos, chickenPos);
        float currentDistance = Vector2.Distance(playerPos, chickenPos);

        playerLastPos = playerPos; 

        return currentDistance < lastDistance;
    }

}

