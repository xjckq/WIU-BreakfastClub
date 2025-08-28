using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ShopItemSlot : MonoBehaviour
{
    public PlayerData playerData;
    private ItemData currentItem;
    private int currentStock;
    public InventorySystem inventorySystem;
    public Image itemImage;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemCostText;
    public TextMeshProUGUI itemStockText;
    public Button buyButton;
    public void InitialiseItem(ItemData item)
    {
        currentItem = item;
        currentStock = Random.Range(1, 4);
        UpdateUI();
    }
    private void UpdateUI()
    {
        if (currentItem != null)
        {
            itemNameText.text = currentItem.itemName;
            itemCostText.text = $"${currentItem.itemCost.ToString()}";
            itemImage.sprite = currentItem.itemImage;
            itemStockText.text = $"Stock: {currentStock}";
            //buyButton.interactable = true;
            bool canAfford = playerData.money >= currentItem.itemCost;
            buyButton.interactable = currentStock > 0 && canAfford;
        }
    }
    public void OnPurchaseButtonClicked()
    {
        if (playerData.inventoryItems.Count >= playerData.maxInventorySize)
        {
            return;
        }
        if (playerData.money >= currentItem.itemCost)
        {
            playerData.AddMoney(-currentItem.itemCost);
            PurchaseItem(currentItem);
            currentStock--;
            UpdateUI();
            //buyButton.interactable = false;
        }
        else
        {
            Debug.LogWarning("Not enough money to purchase this item.");
        }
    }
    private void PurchaseItem(ItemData itemData)
    {

        ItemInstance newItemInstance = new ItemInstance(itemData, null);

        playerData.inventoryItems.Add(newItemInstance);
        FindAnyObjectByType<InventoryUI>()?.RefreshInventoryUI();
    }
}
