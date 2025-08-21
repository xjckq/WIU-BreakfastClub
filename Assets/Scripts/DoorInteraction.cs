using UnityEngine;
using UnityEngine.InputSystem;

public class DoorInteraction : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [SerializeField] private string doorLayerName = "OpenDoor";
    [SerializeField] private string originalLayerName = "Player";

    private bool _isDoorOpened = false;
    private bool _isNearDoor = false;
    private int _doorLayer;
    private int _originalLayer;

    private InputAction _interactAction;
    private Animator _animator;
    void Awake()
    {
        _interactAction = InputSystem.actions.FindAction("Interact");
        if (_interactAction != null)
        {
            _interactAction.started += OnInteract;
        }
        _animator = door.GetComponent<Animator>();
    }

    void Start()
    {
        _doorLayer = LayerMask.NameToLayer(doorLayerName);
        _originalLayer = gameObject.layer;
        if (_doorLayer == -1)
        {
            Debug.LogError($"Layer '{doorLayerName}' not found. Please add this layer in the Unity editor.");
        }
    }
    private void OnInteract(InputAction.CallbackContext context)
    {
        if (_isNearDoor)
        {
            ToggleDoor();
        }
    }
    private void ToggleDoor()
    {
        _isDoorOpened = !_isDoorOpened;

        if (_isDoorOpened)
        {
            gameObject.layer = _doorLayer;
            Debug.Log("open sesame");
            if (_animator != null)
            {
                _animator.SetBool("isOpen", true);
            }
        }
        else
        {
            gameObject.layer = _originalLayer;
            Debug.Log("close sesame");
            if (_animator != null)
            {
                _animator.SetBool("isOpen", false);
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == door)
        {
            _isNearDoor = true;
            Debug.Log("Entered door trigger.");
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == door && !_isDoorOpened)
        {
            _isNearDoor = false;
            Debug.Log("Exited door trigger.");
        }
    }
    void OnEnable()
    {
        if (_interactAction != null)
        {
            _interactAction.Enable();
        }
    }
    void OnDisable()
    {
        if (_interactAction != null)
        {
            _interactAction.Disable();
        }
    }

    void OnDestroy()
    {
        if (_interactAction != null)
        {
            _interactAction.started -= OnInteract;
        }
    }
}
