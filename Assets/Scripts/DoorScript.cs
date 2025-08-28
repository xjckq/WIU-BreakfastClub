using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public Sprite closedSprite;
    public Sprite openSprite;
    public AudioSource audioSource;
    public AudioClip doorOpenSound;
    public AudioClip doorCloseSound;

    private bool isPlayerInRange = false;
    private bool isOpen = false;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private BoxCollider2D boxCollider;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

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
            audioSource.PlayOneShot(doorCloseSound);
            boxCollider.enabled = true;
        }
        else
        {
            spriteRenderer.sprite = openSprite;
            isOpen = true;
            audioSource.PlayOneShot(doorOpenSound);
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
