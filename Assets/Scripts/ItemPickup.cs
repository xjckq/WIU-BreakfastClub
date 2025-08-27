using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    //[SerializeField] private int itemCount = 0;
    private string playerTag = "Player";
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            //Debug.Log("total items: " + itemCount);
            //itemCount += 1;
            Destroy(gameObject);
        }
    }
}
