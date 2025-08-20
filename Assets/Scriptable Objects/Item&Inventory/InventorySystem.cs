using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public InventoryUI inventoryUI;
    public PlayerData playerData;
    public int maxItems = 10;

    void Start()
    {
        // populate from playerData
        if (playerData != null)
            inventoryUI.RefreshInventoryUI();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Item item))
        {
            if (playerData.inventoryItems.Count >= maxItems)
            {
                Debug.Log("Inventory full");
                return;
            }

            // add item to playerData
            playerData.inventoryItems.Add(item.TakeItem());
            inventoryUI.RefreshInventoryUI();
        }
    }

    public void UseItem(int index, GameObject user)
    {
        if (index < 0 || index >= playerData.inventoryItems.Count) return;

        ItemInstance item = playerData.inventoryItems[index];
        item.itemEffect.Use(user);
        playerData.inventoryItems.RemoveAt(index);

        inventoryUI.RefreshInventoryUI();
    }
}