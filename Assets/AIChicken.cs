using Pathfinding;
using UnityEngine;

public class AIChicken : MonoBehaviour
{
    public enum EnemyState
    {
        IDLE,
        CHASE,
        ESCAPE
    };

    public EnemyState CurrentState { get; set; }
    public GameObject target;
    public float chaseRange = 5f;

    // Push settings
    public float pushDetectionRadius = 1.5f;
    public float pushDistance = 1.2f;
    public float pushSpeed = 4f;
    public float pushDuration = 0.8f;

    private AIDestinationSetter aiDest;
    private Rigidbody2D playerRB;

    // Push variables
    private float pushTimer = 0f;
    private Vector2 pushTargetPos;

    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer enemySprite;
    float scaleX;

    void Start()
    {
        aiDest = GetComponent<AIDestinationSetter>();
        playerRB = target.GetComponent<Rigidbody2D>();
        scaleX = transform.localScale.x;

        ChangeState(EnemyState.IDLE);
    }

    void Update()
    {
        pushTimer -= Time.deltaTime;

        if (pushTimer > 0)
        {
            Vector2 currentPos = transform.position;
            Vector2 newPos = Vector2.MoveTowards(currentPos, pushTargetPos, pushSpeed * Time.deltaTime);
            transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
            return;
        }

        switch (CurrentState)
        {
            case EnemyState.IDLE:
                Idle();
                break;
            case EnemyState.CHASE:
                Chase();
                break;
            case EnemyState.ESCAPE:
                Escape();
                break;
        }

        CheckForPush();
    }

    private void CheckForPush()
    {
        Vector2 chickenPos = transform.position;
        Vector2 playerPos = target.transform.position;
        float distanceToPlayer = Vector2.Distance(chickenPos, playerPos);

        if (distanceToPlayer < pushDetectionRadius)
            Push(chickenPos, playerPos);
    }

    private void Push(Vector2 chickenPos, Vector2 playerPos)
    {
        aiDest.target = null;

        Vector2 pushDirection = (chickenPos - playerPos).normalized;

        // set where the chicken will be pushed to
        pushTargetPos = chickenPos + pushDirection * pushDistance;

        // start the push timer
        pushTimer = pushDuration;

        // face away from player
        if (pushDirection.x > 0)
            transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
        else
            transform.localScale = new Vector3(-scaleX, transform.localScale.y, transform.localScale.z);

        ChangeState(EnemyState.ESCAPE);
    }

    private void Idle()
    {
        //animator.SetBool("isMoving", false);

        float distanceToPlayer = Vector3.Distance(target.transform.position, transform.position);

        if (distanceToPlayer < chaseRange)
            ChangeState(EnemyState.CHASE);
    }

    private void Chase()
    {
        //animator.SetBool("isMoving", true);
        Flip();

        float distanceToPlayer = Vector3.Distance(target.transform.position, transform.position);

        if (distanceToPlayer > chaseRange)
            ChangeState(EnemyState.IDLE);
        
    }

    private void Escape()
    {
        float distanceToPlayer = Vector3.Distance(target.transform.position, transform.position);

        if (distanceToPlayer > chaseRange)
            ChangeState(EnemyState.IDLE);
        else if (distanceToPlayer < chaseRange)
            ChangeState(EnemyState.CHASE);
    }

    private void ChangeState(EnemyState nextState)
    {
        switch (nextState)
        {
            case EnemyState.CHASE:
                aiDest.target = target.transform;
                break;

            case EnemyState.IDLE:
                aiDest.target = null;
                break;

            case EnemyState.ESCAPE:
                aiDest.target = null;
                break;
        }
        CurrentState = nextState;
    }

    private void Flip()
    {
        if (target.transform.position.x > transform.position.x)
            transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
        else
            transform.localScale = new Vector3(-scaleX, transform.localScale.y, transform.localScale.z);
    }
}