using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public Sprite closedSprite;
    public Sprite openSprite;

    private bool isPlayerInRange = false;
    private bool isOpen = false;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private BoxCollider2D boxCollider;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = closedSprite;
        boxCollider.enabled = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isPlayerInRange)
        {
            ToggleDoor();
        }
    }

    public void ToggleDoor()
    {
        if (isOpen)
        {
            spriteRenderer.sprite = closedSprite;
            isOpen = false;
            boxCollider.enabled = true;
        }
        else
        {
            spriteRenderer.sprite = openSprite;
            isOpen = true;
            boxCollider.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }
}
