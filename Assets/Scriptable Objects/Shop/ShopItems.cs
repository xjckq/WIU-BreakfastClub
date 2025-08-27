using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopItems", menuName = "Scriptable Objects/ShopItems")]
public class ShopItems : ScriptableObject
{
    public List<ItemData> allItems;
    public float shopRefreshTime = 5.0f;
}
