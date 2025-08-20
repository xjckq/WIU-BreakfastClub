using UnityEngine;
[System.Serializable]

public class MaterialInstance
{
    public MaterialData materialData;

    public MaterialInstance(MaterialData materialData)
    {
        this.materialData = materialData;
    }
}
