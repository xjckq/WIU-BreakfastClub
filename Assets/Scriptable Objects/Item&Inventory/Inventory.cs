using UnityEngine;
using System.Collections.Generic;  

[CreateAssetMenu(fileName = "Inventory", menuName = "Scriptable Objects/Inventory")]
public class Inventory : ScriptableObject
{
    public int maxItems = 4;
    public List<ItemInstance> items = new List<ItemInstance>();

    public bool AddItem(ItemInstance item)
    {
        if (items.Count > maxItems) return false;
        items.Add(item);
        return true;
    }

    public void DisplayItems()
    {
        foreach (ItemInstance item in items)
        {
            Debug.Log($"Item Name: {item.itemData.itemName}");
        }
    }

    public ItemInstance GetItem(int index) { return items[index]; }

    public void RemoveItem(int index)
    {
        items.RemoveAt(index);
    }
}
