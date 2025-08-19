using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    SpriteRenderer spriteRenderer;
    Rigidbody2D body;
    InputAction moveAction;
    Vector2 moveDirection;
    public float speed = 5f;



    void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
       // animator.SetBool("IsMoving", false);
    }

    private void OnEnable()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        moveAction.performed += onMove;
        moveAction.canceled += onMoveCancel;
    }

    private void OnDisable()
    {
        moveAction.performed -= onMove;
        moveAction.canceled -= onMoveCancel;
    }

    private void onMove(InputAction.CallbackContext ctx)
    {
        moveDirection = ctx.ReadValue<Vector2>();
        
    }

    private void onMoveCancel(InputAction.CallbackContext ctx)
    {
        moveDirection = Vector2.zero;
    }

    void FixedUpdate()
    {
        Vector2 movement = moveDirection.normalized * speed;
        body.linearVelocity = movement;
    }
}