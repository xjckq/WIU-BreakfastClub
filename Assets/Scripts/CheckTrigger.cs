using UnityEngine;

public class CheckTrigger : MonoBehaviour
{
    public CollisionTarget[] targets;
    private bool isLoading = false;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (isLoading) return; // already loading, ignore further triggers

        foreach (CollisionTarget target in targets)
        {
            if (other == target.collider)
            {
                target.onCollisionEnter.Invoke();
                isLoading = true; // prevent multiple loads
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
