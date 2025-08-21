using UnityEngine;
using System.Collections;

public enum NPCState
{
    Idle,
    Patrol,
    Chase,
    Attack
}

public class NPCMovement : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 1f;
    public float patrolMinDuration = 5f;
    public float patrolMaxDuration = 8f;
    public float idleMinDuration = 2f;
    public float idleMaxDuration = 5f;
    public float patrolDuration;
    public float idleDuration;

    public float chaseSpeed = 3f;
    public float chaseRange = 10f;
    public float stopChasingDistance = 2f;
    public float attackRange = 2f;
    public int regularAttackDamage = 10;
    public float regularAttackInterval = 1f;

    private Rigidbody2D _rb;
    private NPCState currentState;
    private float stateTimer;
    private Vector2 patrolDirection;
    [SerializeField] BoxCollider2D _collider;
    [SerializeField] BoxCollider2D _NPCCollider;
    [SerializeField] BoxCollider2D _leftCollider;
    [SerializeField] BoxCollider2D _rightCollider;
    [SerializeField] BoxCollider2D _topCollider;
    [SerializeField] BoxCollider2D _bottomCollider;
    [SerializeField] private Transform sprite;
    private Coroutine attackCoroutine;

    [SerializeField] PlayerController playerGO;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
        if (_rb == null)
        {
            enabled = false;
            return;
        }

        ChangeState(NPCState.Idle);
    }

    void Update()
    {

        float _colliderXMin = _collider.bounds.min.x;
        float _colliderXMax = _collider.bounds.max.x;
        float _colliderYMin = _collider.bounds.min.y;
        float _colliderYMax = _collider.bounds.max.y;
        Vector2 NPCSize = _NPCCollider.bounds.size;

        switch (currentState)
        {
            case NPCState.Patrol:
                HandlePatrol();
                break;
            case NPCState.Idle:
                HandleIdle();
                break;
            case NPCState.Chase:
                HandleChase();
                break;
            case NPCState.Attack:
                HandleAttack();
                break;
        }

        //if (transform.position.x <= _colliderXMin)
        //{
        //    _rb.linearVelocity = new Vector2(1, 0) * moveSpeed;
        //}
        //else if (transform.position.x >= _colliderXMax)
        //{
        //    _rb.linearVelocity = new Vector2(-1, 0) * moveSpeed;
        //}
        //else if(transform.position.y <= _colliderYMin)
        //{
        //    _rb.linearVelocity = new Vector2(0, 1) * moveSpeed;
        //}
        //else if(transform.position.y >= _colliderYMax)z
        //{
        //    _rb.linearVelocity = new Vector2(vel.x, -1) * moveSpeed;
        //}
        RotateTowardsMovement();
    }

    public void OnTakeDamage()
    {
        if (currentState != NPCState.Chase && currentState != NPCState.Attack)
        {
            ChangeState(NPCState.Chase);
        }
    }
    private void HandlePatrol()
    {
        if (patrolDuration == 0)
        {
            patrolDuration = Random.Range(patrolMinDuration, patrolMaxDuration);
            RandomisedDirection();
        }
        stateTimer += Time.deltaTime;
        if (stateTimer >= patrolDuration)
        {
            stateTimer = 0;
            if (patrolDuration != 0)
            {
                patrolDuration = 0;
            }
            ChangeState(NPCState.Idle);
        }
    }

    private void HandleIdle()
    {
        if (idleDuration == 0)
        {
            idleDuration = Random.Range(idleMinDuration, idleMaxDuration);
            _rb.linearVelocity = Vector2.zero;
        }
        stateTimer += Time.deltaTime;
        if (stateTimer >= idleDuration)
        {
            stateTimer = 0;
            if (idleDuration != 0)
            {
                idleDuration = 0;
            }
            ChangeState(NPCState.Patrol);
        }
        //Debug.Log("Idle for: " + idleDuration + " seconds, current state: " + currentState);
    }

    private void HandleChase()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            ChangeState(NPCState.Attack);
            return;
        }
        if (distanceToPlayer > chaseRange)
        {
            ChangeState(NPCState.Patrol);
            return;
        }

        if (distanceToPlayer > stopChasingDistance)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            _rb.linearVelocity = direction * chaseSpeed;
        }
        else
        {
            _rb.linearVelocity = Vector2.zero;
        }
        Vector2 chaseDirection = player.position - transform.position;
        if (chaseDirection.magnitude > 0.1f && sprite != null)
        {
            float angle = (Mathf.Atan2(chaseDirection.y, chaseDirection.x) * Mathf.Rad2Deg);
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            sprite.transform.rotation = Quaternion.Slerp(sprite.transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    private void HandleAttack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer > attackRange)
        {
            ChangeState(NPCState.Chase);
            return;
        }

        if (player.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(2, 2, 1);
        }
        else
        {
            transform.localScale = new Vector3(-2, 2, 1);
        }
        if (attackCoroutine == null)
        {
            attackCoroutine = StartCoroutine(RegularAttack());
        }
        //Debug.Log("Attacking player with regular attack damage: " + regularAttackDamage);
    }

    private IEnumerator RegularAttack()
    {
        while (currentState == NPCState.Attack)
        {

            if (player != null)
            {
                //PlayerController playerHealthSystem = player.GetComponent<PlayerController>();
                //if (playerHealthSystem != null)
                //{
                    playerGO.TakeDmg(regularAttackDamage);
                //}
            }

            yield return new WaitForSeconds(regularAttackInterval);
        }
    }

    private void ChangeState(NPCState newState)
    {
        stateTimer = 0f;
        currentState = newState;
    }
    public void RandomisedDirection()
    {
        Vector2 dir = Random.insideUnitCircle.normalized;
        _rb.linearVelocity = dir * moveSpeed;
        //Debug.Log("hello world");
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
    //private void RotateTowardsMovement()
    //{
    //    if (_rb.linearVelocity.magnitude > 0.1f && sprite != null)
    //    {
    //        float angle = Mathf.Atan2(_rb.linearVelocity.y, _rb.linearVelocity.x) * Mathf.Rad2Deg - 90;
    //        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
    //        sprite.transform.rotation = Quaternion.Slerp(sprite.transform.rotation, targetRotation, Time.deltaTime * 5f);
    //    }
    //}
    void OnCollisionEnter2D(Collision2D collision)
    {
        //if (other.CompareTag("LeftBoundingBox"))
        //{
        //    _rb.linearVelocity = new Vector2(1, _rb.linearVelocity.y);
        //}
        //if (other.CompareTag("RightBoundingBox"))
        //{
        //    _rb.linearVelocity = new Vector2(-1, _rb.linearVelocity.y);
        //}
        //if (other.CompareTag("TopBoundingBox"))
        //{
        //    _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, -1);
        //}
        //if (other.CompareTag("BottomBoundingBox"))
        //{
        //    _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, 1);
        //}
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
    //void OnTriggerEnter2D(Collider2D other)
    //{
    //    //if (other.CompareTag("LeftBoundingBox"))
    //    //{
    //    //    _rb.linearVelocity = new Vector2(1, _rb.linearVelocity.y);
    //    //}
    //    //if (other.CompareTag("RightBoundingBox"))
    //    //{
    //    //    _rb.linearVelocity = new Vector2(-1, _rb.linearVelocity.y);
    //    //}
    //    //if (other.CompareTag("TopBoundingBox"))
    //    //{
    //    //    _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, -1);
    //    //}
    //    //if (other.CompareTag("BottomBoundingBox"))
    //    //{
    //    //    _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, 1);
    //    //}
    //    if (other.CompareTag("LeftBoundingBox"))
    //    {
    //        _rb.linearVelocity = new Vector2(1, 0);
    //    }
    //    if (other.CompareTag("RightBoundingBox"))
    //    {
    //        _rb.linearVelocity = new Vector2(-1, 0);
    //    }
    //    if (other.CompareTag("TopBoundingBox"))
    //    {
    //        _rb.linearVelocity = new Vector2(0, -1);
    //    }
    //    if (other.CompareTag("BottomBoundingBox"))
    //    {
    //        _rb.linearVelocity = new Vector2(0, 1);
    //    }
    //}
}