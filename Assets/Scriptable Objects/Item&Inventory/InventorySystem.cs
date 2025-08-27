using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public InventoryUI inventoryUI;
    public PlayerData playerData;

    public List<ItemData> allItemData;

    void Start()
    {
        if (playerData != null)
            inventoryUI.RefreshInventoryUI();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Item item))
        {
            // check inventory for space
            if (playerData.inventoryItems.Count >= playerData.maxInventorySize)
            {
                Debug.Log("Inventory full");
                return;
            }

            ItemInstance picked = item.TakeItem();

            if (picked != null)
            {
                // add to inventory
                playerData.inventoryItems.Add(picked);
                inventoryUI.RefreshInventoryUI();
                // track for quest that requires player to collect item
                QuestManager.Instance.ItemCollected(picked.itemData);
                Destroy(other.gameObject);
                Debug.Log($"Picked up {picked.itemData.itemName}");
            }
            else
            {
                Debug.LogWarning("Item.TakeItem() returned null!");
            }
        }
    }

    public void UseItem(int index)
    {
        if (index < 0 || index >= playerData.inventoryItems.Count) return;

        ItemInstance item = playerData.inventoryItems[index];

        item.itemEffect.Use(playerData);

        playerData.inventoryItems.RemoveAt(index);
        inventoryUI.RefreshInventoryUI();
    }

    public void Update()
    {
        if (playerData.buffFoodActive)
        {
            playerData.buffFoodRemainingTime -= Time.deltaTime;
            // revert speed buff
            if (playerData.buffFoodRemainingTime <= 0f)
            {
                playerData.buffFoodActive = false;
                playerData.currentSpeed = playerData.speed;
                Debug.Log("buff reverted");
            }
        }
    }

    public ItemData GetItemDataByID(string id)
    {
        return allItemData.Find(item => item.itemName == id);
    }
}