using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "MaterialInventory", menuName = "Scriptable Objects/MaterialInventory")]
public class MaterialInventory : ScriptableObject
{
    public int maxMaterials = 4;
    public List<MaterialInstance> materials = new List<MaterialInstance>();

    public bool AddMaterial(MaterialInstance material)
    {
        if (materials.Count >= maxMaterials) return false;
        materials.Add(material);
        return true;
    }

    public void DisplayItems()
    {

        foreach (MaterialInstance material in materials)
        {
            Debug.Log($"Item Name: {material.materialData.materialName}");
        }
    }

    public MaterialInstance GetMaterial(int index) { return materials[index]; }

    public void RemoveMaterial(int index)
    {
        materials.RemoveAt(index);
    }
}
