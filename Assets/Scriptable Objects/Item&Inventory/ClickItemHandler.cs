using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickItemHandler : MonoBehaviour, IPointerClickHandler
{
    public InventoryUI inventoryUI;

    public void OnPointerClick(PointerEventData eventData)
    {
        Button btn = GetComponent<Button>();
        if (!btn) return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            inventoryUI.UseItem(btn);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            inventoryUI.DropItem(btn);
        }
    }
}
