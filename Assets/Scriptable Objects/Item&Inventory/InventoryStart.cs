using UnityEngine;

public class InventoryStart : MonoBehaviour
{
    public PlayerData playerData;
    public InventoryUI inventoryUI;

    void Start()
    {
        playerData.inventoryItems.Clear();
        inventoryUI.RefreshInventoryUI();

        // reset buffs
        inventoryUI.ResetBuffs();
    }
}
