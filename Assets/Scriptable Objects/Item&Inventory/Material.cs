using UnityEngine;

public class Material : MonoBehaviour
{
    public MaterialInstance material;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = material.materialData.materialImage;
    }

    public MaterialInstance TakeMaterial()
    {
        Destroy(gameObject);
        return material;
    }
}
