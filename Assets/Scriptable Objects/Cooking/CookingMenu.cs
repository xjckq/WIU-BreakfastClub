using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CookingMenu : MonoBehaviour
{
    public PlayerData playerData;
    public InventoryUI inventoryUI;

    // UI
    public Image recipeImageUI;
    public TMP_Text recipeNameUI;
    public TMP_Text ingredientsTextUI;
    public TMP_Text feedbackText;

    public GameObject clickPrompt;  
    public Slider cookingProgressBar;  

    // effects
    public Animator panAnimator;
    public ParticleSystem cookingSmoke;

    // buttons
    public Button prevRecipeButton;
    public Button nextRecipeButton;
    public Button cookButton;
    public Button panButton;       

    // recipes
    public List<RecipeData> recipes;
    private int currentRecipeIndex = 0;

    // cooking QTE
    private int requiredClicks = 10;
    private int currentClicks = 0;
    private bool isCooking = false;

    // audio 
    public AudioSource audioSource;
    public AudioClip sizzlingSound;

    private void Start()
    {
        inventoryUI.ResetBuffs();
        cookingProgressBar.gameObject.SetActive(false);
        clickPrompt.SetActive(false);
    }

    void OnEnable()
    {
        inventoryUI.RefreshInventoryUI();
        ShowRecipe(currentRecipeIndex);
        ResetVFX();
    }

    private void ResetVFX()
    {
        if (panAnimator != null)
        {
            panAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
            panAnimator.Play("Idle", -1, 0f);
            panAnimator.SetBool("isCooking", false);
        }

        if (cookingSmoke != null)
        {
            var smoke = cookingSmoke.main;
            smoke.useUnscaledTime = true;
            cookingSmoke.Clear();
            cookingSmoke.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
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
            StartCoroutine(CookRoutine(recipe));
        }
        else
        {
            feedbackText.text = "Not enough ingredients!";
        }
    }

    private IEnumerator CookRoutine(RecipeData recipe)
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

        // show QTE UI
        isCooking = true;
        currentClicks = 0;
        cookingProgressBar.value = 0;
        cookingProgressBar.maxValue = requiredClicks;
        cookingProgressBar.gameObject.SetActive(true);

        clickPrompt.SetActive(true);
        Animator clickPromptAnimator = clickPrompt.GetComponent<Animator>();
        if (clickPromptAnimator != null)
        {
            clickPromptAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
            clickPromptAnimator.Play("Click", -1, 0f);
        }

        // enable pan clicking
        panButton.interactable = true;

        // play VFX
        if (panAnimator != null)
        {
            panAnimator.SetBool("isCooking", true);
            audioSource.PlayOneShot(sizzlingSound);
        }
            
        if (cookingSmoke != null)
        {
            cookingSmoke.Clear();
            cookingSmoke.Play();
        }

        // 5s QTE
        float timer = 5f;
        while (timer > 0f && isCooking)
        {
            timer -= Time.unscaledDeltaTime;
            // succeed if enough clicks
            if (currentClicks >= requiredClicks) break;
            yield return null;
        }

        // stop cooking anim/smoke
        panAnimator.SetBool("isCooking", false);
        audioSource.Stop();
        cookingSmoke.Stop();

        // disable click UI
        clickPrompt.SetActive(false);
        cookingProgressBar.gameObject.SetActive(false);
        panButton.interactable = false;

        // check result
        if (currentClicks >= requiredClicks)
        {
            // add dish to inventory
            ItemInstance dish = new ItemInstance(recipe.resultItem, recipe.resultEffect);
            playerData.inventoryItems.Add(dish);
            inventoryUI.RefreshInventoryUI();
            feedbackText.text = $"Cooked {recipe.recipeName}!";

            if (QuestManager.Instance != null)
            {
                QuestManager.Instance.ItemCrafted(recipe.resultItem);
            }
        }
        else
        {
            // return ingredients
            foreach (var req in recipe.ingredientsNeeded)
            {
                for (int i = 0; i < req.amount; i++)
                    playerData.inventoryItems.Add(new ItemInstance(req.ingredient, null));
            }
            inventoryUI.RefreshInventoryUI();
            feedbackText.text = $"Failed to cook {recipe.recipeName}!";
        }

        isCooking = false;
    }

    public void OnPanClicked()
    {
        if (!isCooking) return;

        currentClicks++;
        cookingProgressBar.value = currentClicks;
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