using UnityEngine;

[CreateAssetMenu(fileName = "ItemEffect", menuName = "Scriptable Objects/ItemEffect")]
public abstract class ItemEffect : ScriptableObject
{
    public abstract void Use(GameObject user);
    public float duration = 10f;
}
