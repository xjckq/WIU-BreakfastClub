using UnityEngine;
using System.Collections;

public enum MonkeyState
{
    Idle,
    Patrol,
    Flee,
    ThrowAttack,
    Chase
}

public class MonkeyMovement : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 1f;
    public float patrolMinDuration = 5f;
    public float patrolMaxDuration = 8f;
    public float idleMinDuration = 2f;
    public float idleMaxDuration = 5f;
    public float fleeSpeed = 2f;
    public float fleeDistance = 5f;
    public float attackRange = 8f;
    public float throwInterval = 1.5f;
    public float chaseSpeed = 2f;
    public float wallCheckDistance = 0.5f;
    public GameObject bananaPrefab;

    private Rigidbody2D _rb;
    private MonkeyState currentState;
    private float stateTimer;
    private float patrolDuration;
    private float idleDuration;
    [SerializeField] private Transform sprite;
    private Coroutine attackCoroutine;
    private float _nextThrowTime = 0f;
    private Animator _animator;
    
    public AudioSource audioSource;
    public AudioClip AttackMonkeySound;

    [SerializeField] PlayerController playerGO;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();

        if (_rb == null)
        {
            enabled = false;
            return;
        }

        ChangeState(MonkeyState.Idle);
    }

    void Update()
    {
        switch (currentState)
        {
            case MonkeyState.Patrol:
                HandlePatrol();
                break;
            case MonkeyState.Idle:
                HandleIdle();
                break;
            case MonkeyState.Flee:
                HandleFlee();
                break;
            case MonkeyState.ThrowAttack:
                HandleThrowAttack();
                break;
            case MonkeyState.Chase:
                HandleChase();
                break;
        }

        RotateTowardsMovement();
    }

    public void OnTakeDamageMonkey()
    {
        ChangeState(MonkeyState.Flee);
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
    }

    private void HandlePatrol()
    {
        _animator.SetBool("isPatrolling", true);

        if (patrolDuration == 0)
        {
            patrolDuration = Random.Range(patrolMinDuration, patrolMaxDuration);
            RandomisedDirection();
        }

        stateTimer += Time.deltaTime;
        if (stateTimer >= patrolDuration)
        {
            stateTimer = 0;
            patrolDuration = 0;
            ChangeState(MonkeyState.Idle);
        }
    }

    private void HandleIdle()
    {
        _animator.SetBool("isPatrolling", false);
        _animator.SetBool("isFleeing", false);
        _animator.SetBool("isChasing", false);

        if (idleDuration == 0)
        {
            idleDuration = Random.Range(idleMinDuration, idleMaxDuration);
            _rb.linearVelocity = Vector2.zero;
        }

        stateTimer += Time.deltaTime;
        if (stateTimer >= idleDuration)
        {
            stateTimer = 0;
            idleDuration = 0;
            ChangeState(MonkeyState.Patrol);
        }
    }

    private void HandleFlee()
    {
        _animator.SetBool("isPatrolling", false);
        _animator.SetBool("isFleeing", true);
        _animator.SetBool("isChasing", false);

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        Vector2 direction = (transform.position - player.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, wallCheckDistance, LayerMask.GetMask("Wall"));
        if (hit.collider != null)
        {
            _rb.linearVelocity = Vector2.zero;
            ChangeState(MonkeyState.ThrowAttack);
            return;
        }
        _rb.linearVelocity = direction * fleeSpeed;
        if (distanceToPlayer >= fleeDistance)
        {
            ChangeState(MonkeyState.ThrowAttack);
            _rb.linearVelocity = Vector2.zero;
        }
    }

    private void HandleThrowAttack()
    {
        _animator.SetBool("isPatrolling", false);
        _animator.SetBool("isFleeing", false);
        _animator.SetBool("isChasing", false);

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (player != null && sprite != null)
        {
            Vector2 direction = player.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            angle -= 90;
            sprite.transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        _rb.linearVelocity = Vector2.zero;

        if (Time.time >= _nextThrowTime)
        {
            if (bananaPrefab != null)
            {
                _animator.SetTrigger("AttackTrigger");
                audioSource.PlayOneShot(AttackMonkeySound);
                GameObject banana = Instantiate(bananaPrefab, transform.position, Quaternion.identity);

                Banana bananaScript = banana.GetComponent<Banana>();
                if (bananaScript != null)
                {
                    Vector2 directionToPlayer = (player.position - transform.position);
                    bananaScript.Launch(directionToPlayer);
                }
            }
            _nextThrowTime = Time.time + throwInterval;
        }


        if (distanceToPlayer <= fleeDistance)
        {
            ChangeState(MonkeyState.Flee);
        }
        else if (distanceToPlayer > attackRange)
        {
            ChangeState(MonkeyState.Chase);
        }
    }

    private void HandleChase()
    {
        _animator.SetBool("isPatrolling", false);
        _animator.SetBool("isFleeing", false);
        //_animator.SetBool("isAttacking", false);
        _animator.SetBool("isChasing", true);
        _rb.linearVelocity = Vector2.zero;
        Vector2 direction = (player.position - transform.position).normalized;
        _rb.linearVelocity = direction * chaseSpeed;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange)
        {
            ChangeState(MonkeyState.ThrowAttack);
        }
    }

    private void ChangeState(MonkeyState newState)
    {
        if (currentState == newState)
        {
            return;
        }
        stateTimer = 0f;
        currentState = newState;
    }

    public void RandomisedDirection()
    {
        Vector2 dir = Random.insideUnitCircle.normalized;
        _rb.linearVelocity = dir * moveSpeed;
    }

    private void RotateTowardsMovement()
    {
        if (_rb.linearVelocity.magnitude > 0.1f && sprite != null)
        {
            float angle = Mathf.Atan2(_rb.linearVelocity.y, _rb.linearVelocity.x) * Mathf.Rad2Deg;
            angle -= 90;
            sprite.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
    public void SetPlayer(Transform newPlayer)
    {
        player = newPlayer;
    }
    public void SetPlayerHealth(PlayerController newPlayerGO)
    {
        playerGO = newPlayerGO;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("LeftBoundingBox"))
        {
            _rb.linearVelocity = new Vector2(1, 0);
        }
        if (collision.gameObject.CompareTag("RightBoundingBox"))
        {
            _rb.linearVelocity = new Vector2(-1, 0);
        }
        if (collision.gameObject.CompareTag("TopBoundingBox"))
        {
            _rb.linearVelocity = new Vector2(0, -1);
        }
        if (collision.gameObject.CompareTag("BottomBoundingBox"))
        {
            _rb.linearVelocity = new Vector2(0, 1);
        }
    }
}
