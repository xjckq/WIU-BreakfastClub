using Pathfinding;
using System;
using System.Collections;
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
    public float chaseRange;

    private AIDestinationSetter aiDest;

    [SerializeField] Animator animator;

    [SerializeField] SpriteRenderer enemySprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        aiDest = GetComponent<AIDestinationSetter>();
        ChangeState(EnemyState.IDLE);
    }

    // Update is called once per frame
    void Update()
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
            default:
                break;
        }

    }



    private void Idle()
    {

       // animator.SetBool("isMoving", false);

        if (Vector3.Distance(target.transform.position, transform.position) < chaseRange)
        {
            ChangeState(EnemyState.CHASE);
        }
    }
    private void Chase()
    {

       // animator.SetBool("isMoving", true);

        if (Vector3.Distance(target.transform.position, transform.position) > chaseRange)
        {
            ChangeState(EnemyState.IDLE);
        }


    }

    private void Escape()
    {
        // move away from the target
        Vector3 direction = (transform.position - target.transform.position).normalized;
        Vector3 escapePosition = transform.position + direction * chaseRange;
        aiDest.target.position = escapePosition;
    }
  
    private void ChangeState(EnemyState nextState)
    {
        switch (nextState)
        {
            case EnemyState.CHASE:
                {
                    Debug.Log("Chase");
                    aiDest.target = target.transform;
                }
                break;
            case EnemyState.IDLE:
                {
                    Debug.Log("Idle");
                    aiDest.target = null;
                }
                break;
        }
        CurrentState = nextState;
    }


}
