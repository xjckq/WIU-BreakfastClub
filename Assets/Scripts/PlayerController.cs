using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    SpriteRenderer spriteRenderer;
    Rigidbody2D body;
    InputAction moveAction;
    Vector2 moveDirection;
    public float speed = 5;


    void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
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

        animator.SetBool("isMovingSide", false);
        animator.SetBool("isMovingUp", false);
        animator.SetBool("isMovingDown", false);

        if (moveDirection.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            animator.SetBool("isMovingSide", true);
        }
        else if (moveDirection.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            animator.SetBool("isMovingSide", true);
        }
        else
        {
            if (moveDirection.y < 0)
                animator.SetBool("isMovingDown", true);
            else if (moveDirection.y > 0)
                animator.SetBool("isMovingUp", true);
        }
    }


    private void onMoveCancel(InputAction.CallbackContext ctx)
    {
        moveDirection = Vector2.zero;
        body.linearVelocity = Vector2.zero;

        animator.SetBool("isMovingUp", false);
        animator.SetBool("isMovingDown", false);
        animator.SetBool("isMovingSide", false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))


            QuestManager.Instance.killEnemyCount++;

        if (QuestManager.Instance.killEnemyCount == 3)
        {
            Debug.Log("Quest Done");
        }


    }

    void FixedUpdate()
    {
        Vector2 movement = moveDirection.normalized * speed;
        body.linearVelocity = movement;
    }


}
