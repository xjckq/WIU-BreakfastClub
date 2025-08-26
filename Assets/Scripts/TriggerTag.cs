using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;

[System.Serializable]
public class CollisionTagName
{
    public string tagName;
    public UnityEvent onCollisionEnter;
    public UnityEvent onCollisionExit;
}

public class TriggerTag : MonoBehaviour
{
    public CollisionTagName[] targets;

    public void OnTriggerEnter2D(Collider2D other)
    {
        foreach (CollisionTagName target in targets)
        {
            if (other.CompareTag(target.tagName))
            {
                target.onCollisionEnter.Invoke();
                return;
            }
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        foreach(CollisionTagName target in targets)
        {
            if (other.CompareTag(target.tagName))
            {
                target.onCollisionExit.Invoke();
                return;
            }
        }
    }
}