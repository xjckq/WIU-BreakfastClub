using UnityEngine;
using UnityEngine.InputSystem;

public class DoorInteraction : MonoBehaviour
{
    [HideInInspector] public bool IsPlayerNear = false;

    [SerializeField] private string openDoorLayerName = "OpenDoor";

    private bool _isDoorOpened = false;
    private int _openDoorLayer;
    private int _originalLayer;

    private Animator _animator;

    private InputAction _interactAction;

    void Awake()
    {
        _interactAction = InputSystem.actions.FindAction("Interact");
        if (_interactAction != null)
        {
            _interactAction.started += OnInteract;
        }
        _animator = GetComponent<Animator>();
        Debug.Log("Togle door: awake");
    }

    void Start()
    {
        _openDoorLayer = LayerMask.NameToLayer(openDoorLayerName);
        _originalLayer = gameObject.layer;

        if (_openDoorLayer == -1)
        {
            Debug.LogError($"Layer '{openDoorLayerName}' not found. Please add this layer in the Unity editor.");
        }
        Debug.Log("Togle door: start");
    }

    private void Update()
    {
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (IsPlayerNear)
        {
            ToggleDoor();
        }
    }

    private void ToggleDoor()
    {
        _isDoorOpened = !_isDoorOpened;
        if (_isDoorOpened)
        {
            gameObject.layer = _openDoorLayer;
            Debug.Log("Door opened Layer changed to: " + openDoorLayerName);
            if (_animator != null)
            {
                //_animator.SetBool("isOpen", true);
                _animator.SetTrigger("triggerOpen");
            }
        }
        else
        {
            gameObject.layer = _originalLayer;
            Debug.Log("Door closed Layer changed to original.");
            if (_animator != null)
            {
                //_animator.SetBool("isOpen", false);
                _animator.SetTrigger("triggerClose");
            }
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
