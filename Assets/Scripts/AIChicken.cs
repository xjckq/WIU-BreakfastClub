using Pathfinding;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AIChicken : MonoBehaviour
{
    public enum EnemyState
    {
        IDLE,
        CHASE,
        ESCAPE,
        CAPTURE,
        RUNAWAY
    };

    public EnemyState CurrentState { get; set; }
    public GameObject target;
    public float chaseRange = 5;

    public CTCManager manager;

    public float pushRadius = 1.5f;
    public float pushForce = 15; 
    public float pushDuration = 0.8f;
    public float drag = 8;

    AIDestinationSetter aiDest;
    Rigidbody2D playerRB;
    Rigidbody2D chickenRB; 

    float pushTimer = 0;
    bool isPushing = false;
    Vector2 pushDirection;

    bool isCaptured;

    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer enemySprite;
    float scaleX;

    void Start()
    {
        aiDest = GetComponent<AIDestinationSetter>();
        playerRB = target.GetComponent<Rigidbody2D>();
        chickenRB = GetComponent<Rigidbody2D>();
        scaleX = transform.localScale.x;

        ChangeState(EnemyState.IDLE);
    }

    void Update()
    {
        if (CurrentState == EnemyState.CAPTURE)
            return;

        pushTimer -= Time.deltaTime;
        
        // apply push force when is getting pushed and timer is active
        if (isPushing && pushTimer > 0)
            chickenRB.AddForce(pushDirection * pushForce, ForceMode2D.Force);

        // stop pushing and change state back to chase/idle when timer is up
        if (pushTimer <= 0 && isPushing)
        {
            isPushing = false;

            if (CurrentState == EnemyState.ESCAPE)
            {
                float distanceToPlayer = Vector3.Distance(target.transform.position, transform.position);
                if (distanceToPlayer > chaseRange)
                    ChangeState(EnemyState.IDLE);
                else
                    ChangeState(EnemyState.CHASE);
            }
        }

        if (!isPushing)
        {
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
                case EnemyState.RUNAWAY:
                    Destroy(gameObject);
                    break;
                default:
                    break;
            }
        }

        Push();
    }

    private void Push()
    {
        if (isPushing)
            return;

        Vector2 chickenPos = transform.position;
        Vector2 playerPos = target.transform.position;
        float distanceToPlayer = Vector2.Distance(chickenPos, playerPos);

        // if player is within push radius then push the chicken away
        if (distanceToPlayer < pushRadius)
            Push(chickenPos, playerPos);
    }

    private void Push(Vector2 chickenPos, Vector2 playerPos)
    {
        aiDest.target = null;

        pushDirection = (chickenPos - playerPos).normalized;

        pushTimer = pushDuration;
        isPushing = true;

        //face away from player
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

            case EnemyState.CAPTURE:
                aiDest.target = null;
                manager.Captured();
                break;
            case EnemyState.RUNAWAY:
                aiDest.target = null;
                manager.Escaped();
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "CaptureZone")
        {
            if (isCaptured)
                return;
            Debug.Log("capture zone testest");
            isCaptured = true;
            ChangeState(EnemyState.CAPTURE);
        }

        if (collision.gameObject.tag == "EscapeZone")
        {
            Debug.Log("escape zone testest");
            ChangeState(EnemyState.RUNAWAY);
        }
    }
}