using UnityEngine;

[CreateAssetMenu(fileName = "IngredientsNeeded", menuName = "Scriptable Objects/IngredientsNeeded")]
public class IngredientsNeeded : ScriptableObject
{
    public ItemData ingredient;
    public int amount;
}
