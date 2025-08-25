using UnityEngine;

[CreateAssetMenu(fileName = "RecipeData", menuName = "Scriptable Objects/RecipeData")]
public class RecipeData : ScriptableObject
{
    public string recipeName;
    public Sprite recipeImage;
    public IngredientsNeeded[] ingredientsNeeded;
    public ItemData resultItem;
    public ItemEffect resultEffect;
}
