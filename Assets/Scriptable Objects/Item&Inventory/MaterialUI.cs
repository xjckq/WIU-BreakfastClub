using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MaterialUI : MonoBehaviour
{
    public PlayerData playerData;
    public MaterialSystem materialSystem;
    public InventoryUI inventoryUI;

    // UI
    public Transform materialCell;
    public TMP_Text feedbackText;

    // materials
    public MaterialData flower;
    public MaterialData pear;
    public MaterialData apple;
    public MaterialData banana;

    // potions
    public ItemData healPotionItem;
    public ItemEffect healPotionEffect;
    public ItemData buffPotionItem;
    public ItemEffect buffPotionEffect;

    void Start()
    {
        // populate from playerData
        if (playerData != null && playerData.materialInventoryItems.Count > 0)
        {
            foreach (var material in playerData.materialInventoryItems)
            {
                AddItemToMaterialGUI(material.materialData.materialImage);
            }
        }
    }

    public void AddItemToMaterialGUI(Sprite newMaterial)
    {
        foreach (Transform cell in materialCell)
        {
            Debug.Log(cell);
            Image materialImage = cell.GetComponentInChildren<Image>();

            if (materialImage != null && materialImage.sprite == null)
            {
                materialImage.sprite = newMaterial;
                materialImage.enabled = true;
                materialImage.color = Color.white;
                break; 
            }
        }
    }

    public void AddMaterialToplayerData(MaterialInstance material)
    {
        if (playerData != null)
           playerData.materialInventoryItems.Add(material);
    }

    public void RemoveMaterialFromplayerData(MaterialData mat)
    {
        if (playerData == null) return;

        for (int i = 0; i < playerData.materialInventoryItems.Count; i++)
        {
            if (playerData.materialInventoryItems[i].materialData == mat)
            {
                playerData.materialInventoryItems.RemoveAt(i);
                return; // remove one at a time
            }
        }
    }

    public void RefreshMaterialUI()
    {
        // clear all cells first
        foreach (Transform cell in materialCell)
        {
            Image img = cell.GetComponentInChildren<Image>();
            if (img != null)
            {
                img.sprite = null;
                img.color = new Color(0.7176471f, 0.4431373f, 0.4431373f, 1f);
            }
        }

        // repopulate from playerData
        for (int i = 0; i < playerData.materialInventoryItems.Count; i++)
        {
            AddItemToMaterialGUI(playerData.materialInventoryItems[i].materialData.materialImage);
        }
    }

    // crafting
    public void CraftHealingPotion()
    {
        if (HasMaterials(flower, 2) && HasMaterials(pear, 1))
        {
            if (playerData.inventoryItems.Count < inventoryUI.potionCell.childCount)
            {
                ConsumeMaterials(flower, 2);
                ConsumeMaterials(pear, 1);

                ItemInstance potion = new ItemInstance(healPotionItem, healPotionEffect);
                playerData.inventoryItems.Add(potion);

                inventoryUI.RefreshInventoryUI();
                RefreshMaterialUI();

                feedbackText.text = "Crafted healing potion!";
            }
            else
            {
                feedbackText.text = "No space for potion!";
            }
        }
        else
        {
            feedbackText.text = "Need more materials!";
        }
    }

    public void CraftBuffPotion()
    {
        if (HasMaterials(apple, 2) && HasMaterials(banana, 1))
        {
            if (playerData.inventoryItems.Count < inventoryUI.potionCell.childCount)
            {
                ConsumeMaterials(apple, 2);
                ConsumeMaterials(banana, 1);

                ItemInstance potion = new ItemInstance(buffPotionItem, buffPotionEffect);
                playerData.inventoryItems.Add(potion);

                inventoryUI.RefreshInventoryUI();
                RefreshMaterialUI();

                feedbackText.text = "Crafted Buff potion!";
            }
            else
            {
                feedbackText.text = "No space for potion!";
            }
        }
        else
        {
            feedbackText.text = "Need more materials!";
        }
    }

    private bool HasMaterials(MaterialData mat, int amount)
    {
        if (playerData == null) return false;

        int count = 0;
        for (int i = 0; i < playerData.materialInventoryItems.Count; i++)
        {
            if (playerData.materialInventoryItems[i].materialData == mat)
                count++;
        }

        return count >= amount;
    }

    private void ConsumeMaterials(MaterialData mat, int amount)
    {
        for (int i = 0; i < amount; i++)
            RemoveMaterialFromplayerData(mat);
    }
}