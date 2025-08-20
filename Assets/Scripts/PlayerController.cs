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

    public PlayerData playerData;
    [SerializeField] GameObject hitbox;

    void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        ResetData();
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
        // -- all for testing purposes --
        if (Input.GetKeyDown(KeyCode.C))
            QuestManager.Instance.EnemyKilled();

        if (Input.GetKeyDown(KeyCode.V))
            QuestManager.Instance.ItemCollected();

        if (Input.GetKeyDown(KeyCode.F))
            hitbox.SetActive(true);
        else
            hitbox.SetActive(false);
            
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDmg(5);
            Debug.Log("player has " + playerData.health + "now");
        }

                 Debug.Log("player has " + playerData.health + "now");
    }

    void FixedUpdate()
    {
        Vector2 movement = moveDirection.normalized * speed;
        body.linearVelocity = movement;
    }

    public void AddMoney(int amt)
    {
        playerData.money += amt;
    }

    public void TakeDmg(int dmg)
    {
        playerData.health -= dmg;
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
