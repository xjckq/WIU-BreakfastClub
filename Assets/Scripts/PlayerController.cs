using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    SpriteRenderer spriteRenderer;
    Rigidbody2D body;
    InputAction moveAction;
    InputAction attackAction;
    Vector2 moveDirection;

    public PlayerData playerData;


    bool isAttacking, isMoving;
    Vector2 facingDir = Vector2.down;

    public HealthbarScript healthbar;


    void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();

        // init speed
        if (playerData != null)
            playerData.currentSpeed = playerData.speed;
    }

    void Start()
    {
        ResetData();
        healthbar.SetMaxHealth(playerData.maxHealth);
        healthbar.SetHealth(playerData.health);
    }

    private void OnEnable()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        moveAction.performed += onMove;
        moveAction.canceled += onMoveCancel;

        attackAction = InputSystem.actions.FindAction("Attack");
        attackAction.started += onAttackStart;
        attackAction.canceled+= onAttackEnd;
    }

    private void OnDisable()
    {
        moveAction.performed -= onMove;
        moveAction.canceled -= onMoveCancel;
        attackAction.started -= onAttackStart;
        attackAction.canceled -= onAttackEnd;
    }

    private void onMove(InputAction.CallbackContext ctx)
    {
        if (isAttacking)
            return;

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

        if (moveDirection != Vector2.zero)
            facingDir = moveDirection;

        isMoving = true;
    }


    private void onMoveCancel(InputAction.CallbackContext ctx)
    {
        moveDirection = Vector2.zero;
        body.linearVelocity = Vector2.zero;

        animator.SetBool("isMovingUp", false);
        animator.SetBool("isMovingDown", false);
        animator.SetBool("isMovingSide", false);

        isMoving = false;
    }

    private void onAttackStart(InputAction.CallbackContext ctx)
    {
        if (isMoving)
            return;

        if (facingDir.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            animator.SetTrigger("isAttackSide");
        }
        else if (facingDir.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            animator.SetTrigger("isAttackSide");
        }
        else
        {
            if (facingDir.y < 0)
                animator.SetTrigger("isAttackDown");
            else 
                animator.SetTrigger("isAttackUp");
        }

        isAttacking = true;
    }

    private void onAttackEnd (InputAction.CallbackContext ctx)
    {
        animator.ResetTrigger("isAttackDown");
        animator.ResetTrigger("isAttackUp");
        animator.ResetTrigger("isAttackSide");
        isAttacking = false;
    }


    void Update()
    {
        // -- all for testing purposes --
        if (Input.GetKeyDown(KeyCode.C))
            QuestManager.Instance.EnemyKilled();

        if (Input.GetKeyDown(KeyCode.V))
            QuestManager.Instance.ItemCollected();

        if (Input.GetKeyDown(KeyCode.G))
            QuestManager.Instance.ItemCrafted();

        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDmg(5);
            Debug.Log("player has " + playerData.health + "now");
        }

    }

    void FixedUpdate()
    {

        Vector2 movement = moveDirection.normalized * playerData.currentSpeed;

        if (isAttacking)
        {
            body.linearVelocity = Vector2.zero;
            return;
        }

        //Vector2 movement = moveDirection.normalized * speed;
        body.linearVelocity = movement;

    }

    public void AddMoney(int amt)
    {
        playerData.money += amt;
    }

    public void TakeDmg(int dmg)
    {
        playerData.health -= dmg;
        healthbar.SetHealth(playerData.health);
        if (playerData.health < 0)
            playerData.health = 0;
    }

    public void Heal(int amt)
    {

        playerData.health += amt;
        if (playerData.health > playerData.maxHealth)
            playerData.health = playerData.maxHealth;
    }

    public void ResetData()
    {
        playerData.health = 100;
        playerData.money = 0;
        playerData.maxHealth = 100;
    }
}
