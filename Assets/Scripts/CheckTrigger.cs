using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CollisionTarget
{
    public Collider2D collider;
    public UnityEvent onCollisionEnter;
    public UnityEvent onCollisionExit;
}

public class CheckTrigger : MonoBehaviour
{
    public CollisionTarget[] targets;

    public void OnTriggerEnter2D(Collider2D other)
    {
        foreach (CollisionTarget target in targets)
        {
            if (other == target.collider)
            {
                Debug.Log("Touched: " + target.collider.name);
                target.onCollisionEnter.Invoke();
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
                return;
            }
        }
    }
}