using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public InventoryUI inventoryUI;
    public PlayerData playerData;

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
        Debug.Log("i love ashlyn");
        if (playerData.buffPotionActive)
        {
            playerData.buffPotionRemainingTime -= Time.deltaTime;
            Debug.Log("TESTYTYTYYYT");
            // revert speed buff
            if (playerData.buffPotionRemainingTime <= 0f)
            {
                playerData.buffPotionActive = false;
                playerData.currentSpeed -= 2;
                Debug.Log("buff reverted");
            }
        }
    }
}