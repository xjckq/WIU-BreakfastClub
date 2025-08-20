using System.Collections.Generic;
using UnityEngine;

public class MaterialSystem : MonoBehaviour
{
    public MaterialUI materialUI;
    public PlayerData playerData;
    public int maxMaterials = 10;

    void Start()
    {
        // populate from playerData
        if (playerData != null)
            materialUI.RefreshMaterialUI();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Material material))
        {
            if (PlayerData.materialInventoryItems.Count >= maxMaterials)
            {
                Debug.Log("Material inventory full");
                return;
            }

            // add material to playerData
            Debug.Log("item got picked up");
            PlayerData.materialInventoryItems.Add(material.TakeMaterial());
            materialUI.RefreshMaterialUI();
        }
    }

    public void UseItem(int index, GameObject user)
    {
        if (index < 0 || index >=PlayerData.materialInventoryItems.Count) return;

        MaterialInstance item =PlayerData.materialInventoryItems[index];
       PlayerData.materialInventoryItems.RemoveAt(index);

        materialUI.RefreshMaterialUI();
    }
}