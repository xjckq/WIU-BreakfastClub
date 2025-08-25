using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CollisionTarget
{
    public Collider2D collider;

    public UnityEvent onCollisionEnter;
    public UnityEvent onCollisionExit;
    public TriggerType triggerType;
}

public enum TriggerType
{
    Auto,
    Interactable

}

public class CheckTrigger : MonoBehaviour
{

    public CollisionTarget[] targets;
    private bool isLoading = false;
    private Collider2D interactCollider;

    void Update()
    {
        // check for F key while inside trigger
        if (interactCollider != null && Input.GetKeyDown(KeyCode.E))
        {
                foreach (CollisionTarget target in targets)
                {
                    if (interactCollider == target.collider && target.triggerType == TriggerType.Interactable)
                    {
                        Debug.Log("collider is interacted with");
                        target.onCollisionEnter.Invoke();
                        return;
                    }
                }
        }
    }


    public void OnTriggerEnter2D(Collider2D other)
    {

        if (isLoading) return; // already loading, ignore further triggers

        foreach (CollisionTarget target in targets)
        {
            if (other == target.collider)
            {
                if (target.triggerType == TriggerType.Auto)
                {
                    Debug.Log("Collision");
                    target.onCollisionEnter.Invoke();
                    if (gameObject.layer != LayerMask.NameToLayer("Player"))
                    {
                        isLoading = true; // prevent multiple loads
                    }
                }

                if (target.triggerType == TriggerType.Interactable)
                {
                    Debug.Log("collision with interactable collider");
                    interactCollider = other;
                }
                return;
            }
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        foreach (CollisionTarget target in targets)
        {
            if (other == target.collider)
            {
                target.onCollisionExit.Invoke();
                if (other == interactCollider)
                    interactCollider = null;

                return;
            }
        }
    }
}
