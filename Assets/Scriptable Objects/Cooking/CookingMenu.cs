using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CookingMenu : MonoBehaviour
{
    public PlayerData playerData;
    public InventoryUI inventoryUI;

    // UI
    public Image recipeImageUI;
    public TMP_Text recipeNameUI;
    public TMP_Text ingredientsTextUI;
    public TMP_Text feedbackText;

    // buttons
    public Button prevRecipeButton;
    public Button nextRecipeButton;
    public Button cookButton;

    // recipes
    public List<RecipeData> recipes;
    private int currentRecipeIndex = 0;

    private void Start()
    {
        inventoryUI.ResetBuffs();
    }

    void OnEnable()
    {
        // refresh inventory display whenever the menu is opened
        inventoryUI.RefreshInventoryUI();
        ShowRecipe(currentRecipeIndex);
    }

    public void ShowRecipe(int index)
    {
        if (recipes.Count == 0) return;

        currentRecipeIndex = (index + recipes.Count) % recipes.Count;
        RecipeData recipe = recipes[currentRecipeIndex];

        recipeImageUI.sprite = recipe.recipeImage;
        recipeNameUI.text = recipe.recipeName;

        string ingredientsText = "";
        foreach (var req in recipe.ingredientsNeeded)
        {
            int owned = CountIngredients(req.ingredient);
            ingredientsText += $" x{req.amount} {req.ingredient.itemName}\n";
        }
        ingredientsTextUI.text = ingredientsText;
        feedbackText.text = "";
    }

    public void ShowPrevRecipe() => ShowRecipe(currentRecipeIndex - 1);
    public void ShowNextRecipe() => ShowRecipe(currentRecipeIndex + 1);

    public void CookCurrentRecipe()
    {
        RecipeData recipe = recipes[currentRecipeIndex];

        if (HasIngredients(recipe))
        {
            // consume ingredients
            foreach (var req in recipe.ingredientsNeeded)
            {
                for (int i = 0; i < req.amount; i++)
                {
                    int index = playerData.inventoryItems.FindIndex(x => x.itemData == req.ingredient);
                    if (index >= 0)
                        inventoryUI.RemoveItemFromPlayerData(index);
                }
            }

            // add to player inventory
            ItemInstance dish = new ItemInstance(recipe.resultItem, recipe.resultEffect);
            playerData.inventoryItems.Add(dish);

            // refresh UI
            inventoryUI.RefreshInventoryUI();

            feedbackText.text = $"Cooked {recipe.recipeName}!";
        }
        else
        {
            feedbackText.text = "Not enough ingredients!";
        }

        // refresh display
        ShowRecipe(currentRecipeIndex);
    }

    private bool HasIngredients(RecipeData recipe)
    {
        foreach (var req in recipe.ingredientsNeeded)
        {
            if (CountIngredients(req.ingredient) < req.amount)
                return false;
        }
        return true;
    }

    private int CountIngredients(ItemData item)
    {
        int count = 0;
        foreach (var itemInstance in playerData.inventoryItems)
            if (itemInstance.itemData == item) count++;
        return count;
    }
}
