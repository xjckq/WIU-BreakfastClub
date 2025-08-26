using UnityEngine;

public class DoorTarget : MonoBehaviour
{
    private DoorInteraction _doorInteraction;

    void Awake()
    {
        _doorInteraction = GetComponentInParent<DoorInteraction>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && _doorInteraction != null)
        {
            _doorInteraction.IsPlayerNear = true;
            Debug.Log("Player near door.");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && _doorInteraction != null)
        {
            _doorInteraction.IsPlayerNear = false;
            Debug.Log("Player left door area.");
        }
    }
}
