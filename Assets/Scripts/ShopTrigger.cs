using UnityEngine;
using UnityEngine.InputSystem;

public class ShopTrigger : MonoBehaviour
{
    public GameObject shopUIPanel;
    private bool isPlayerInRange = false;
    public string playerTag = "Player";
    private InputAction interactAction;

    private void Awake()
    {
        interactAction = InputSystem.actions.FindAction("Interact");
        if (interactAction != null)
        {
            interactAction.started += OnInteract;
        }
        if (shopUIPanel != null)
        {
            shopUIPanel.SetActive(false);
        }
    }
    private void OnInteract(InputAction.CallbackContext context)
    {
        if (isPlayerInRange)
        {
            shopUIPanel.SetActive(!shopUIPanel.activeSelf);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            isPlayerInRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            isPlayerInRange = false;
            if (shopUIPanel != null && shopUIPanel.activeSelf)
            {
                shopUIPanel.SetActive(false);
            }
        }
    }
    private void OnEnable()
    {
        if (interactAction != null)
        {
            interactAction.Enable();
        }
    }
    private void OnDisable()
    {
        if (interactAction != null)
        {
            interactAction.Disable();
        }
    }

    private void OnDestroy()
    {
        if (interactAction != null)
        {
            interactAction.started -= OnInteract;
        }
    }
}
