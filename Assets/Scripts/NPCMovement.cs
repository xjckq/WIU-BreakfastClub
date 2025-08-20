using UnityEngine;
using System.Collections;
using JetBrains.Annotations;

public enum NPCState
{
    Idle,
    Patrol
}

public class NPCMovement : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float patrolMinDuration = 5f;
    public float patrolMaxDuration = 8f;
    public float idleMinDuration = 2f;
    public float idleMaxDuration = 5f;
    public float patrolDuration;
    public float idleDuration;
    private Rigidbody2D _rb;
    private NPCState currentState;
    private float stateTimer;
    private Vector3 patrolDirection = Vector3.right;
    [SerializeField] BoxCollider2D _collider;
    [SerializeField] BoxCollider2D _NPCCollider;
    [SerializeField] BoxCollider2D _leftCollider;
    [SerializeField] BoxCollider2D _rightCollider;
    [SerializeField] BoxCollider2D _topCollider;
    [SerializeField] BoxCollider2D _bottomCollider;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
        if (_rb == null)
        {
            enabled = false;
            return;
        }

        ChangeState(NPCState.Patrol);
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
        //else if(transform.position.y >= _colliderYMax)
        //{
        //    _rb.linearVelocity = new Vector2(vel.x, -1) * moveSpeed;
        //}
    }

    private void HandlePatrol()
    {
        if (patrolDuration == 0)
        {
            patrolDuration = Random.Range(patrolMinDuration, patrolMaxDuration);
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
        }
        stateTimer += Time.deltaTime;
        if (stateTimer >= idleDuration)
        {

            patrolDirection.x *= -1;
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            stateTimer = 0;
            if (idleDuration != 0)
            {
                idleDuration = 0;
            }
            ChangeState(NPCState.Patrol);
        }
    }

    private void ChangeState(NPCState newState)
    {
        stateTimer = 0f;
        currentState = newState;
        switch (newState)
        {
            case NPCState.Patrol:
                RandomisedDirection();
                break;
            case NPCState.Idle:
                _rb.linearVelocity = Vector2.zero;
                break;
        }
    }

    public void RandomisedDirection()
    {
        Vector2 dir = Random.insideUnitCircle.normalized;
        _rb.linearVelocity = new Vector2(0 ,0);
        _rb.linearVelocity = dir * moveSpeed;
        Debug.Log("hello world");
    }

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