using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventoryUI : MonoBehaviour
{
    public PlayerData playerData;
    public InventorySystem inventorySystem;

    // potions
    public Transform potionCell;
    public bool buffPotionInUse = false;
    public TMP_Text potionInUseText;
    public TMP_Text HPfullText;

    void Start()
    {
        // populate from playerData
        if (playerData != null && playerData.inventoryItems.Count > 0)
        {
            foreach (var item in playerData.inventoryItems)
                AddItemToInventoryGUI(item.itemData.itemImage);
        }

        // restore buff if active
        if (playerData != null && playerData.buffPotionActive)
        {
            buffPotionInUse = true;
            potionInUseText.gameObject.SetActive(true);

            StartCoroutine(BuffPotionTimer(playerData.buffPotionRemainingTime));
        }
    }

    void Update()
    {
        // hide HP full text if HP not full
        if (HPfullText.gameObject.activeSelf && playerData.health < playerData.maxHealth)
        {
            Debug.Log("HP not full, hiding HP full text");
            HPfullText.gameObject.SetActive(false);
        }
    }

    public void AddItemToInventoryGUI(Sprite newItem)
    {
        foreach (Transform cell in potionCell)
        {
            Image itemImage = cell.GetComponentInChildren<Image>();

            // replace item image in empty slot
            if (itemImage != null && itemImage.sprite == null)
            {
                itemImage.sprite = newItem;
                itemImage.enabled = true;
                itemImage.color = Color.white;
                break;
            }
        }
    }

    public void AddItemToPlayerData(ItemInstance item)
    {
        if (playerData != null)
            playerData.inventoryItems.Add(item);
    }

    public void RemoveItemFromPlayerData(int index)
    {
        if (playerData != null && index >= 0 && index < playerData.inventoryItems.Count)
            playerData.inventoryItems.RemoveAt(index);
    }

    private IEnumerator BuffPotionTimer(float duration)
    {
        float remaining = duration;
        while (remaining > 0)
        {
            playerData.buffPotionRemainingTime = remaining;
            remaining -= Time.deltaTime;
            yield return null;
        }

        buffPotionInUse = false;
        playerData.buffPotionActive = false;
        playerData.buffPotionRemainingTime = 0;

        potionInUseText.gameObject.SetActive(false);
    }

    public void UseItem(Button selectedItemButton)
    {
        if (playerData == null) return;

        Image currentImage = selectedItemButton.GetComponentInChildren<Image>();
        if (currentImage.sprite != null)
        {
            // buff potion check
            if (currentImage.sprite.name == "majulah")
            {
                if (buffPotionInUse)
                {
                    Debug.Log("buff potion already used");
                    return;
                }
                else
                {
                    buffPotionInUse = true;
                    playerData.buffPotionActive = true;
                    playerData.buffPotionRemainingTime = 10f;

                    potionInUseText.gameObject.SetActive(true);
                    StartCoroutine(BuffPotionTimer(10));
                }
            }

            // heal potion check
            if (currentImage.sprite.name == "fatto_idle1_0")
            {
                Debug.Log("teto");
                if (playerData.health >= playerData.maxHealth)
                {
                    Debug.Log("HP full");
                    HPfullText.gameObject.SetActive(true);
                    return;
                }
            }

            int index = int.Parse(selectedItemButton.name[selectedItemButton.name.Length - 1].ToString()) - 1;

            // use item
            if (index >= 0 && index < playerData.inventoryItems.Count)
            {
                ItemInstance item = playerData.inventoryItems[index];

                // apply effect
                item.itemEffect.Use(playerData);
                playerData.inventoryItems.RemoveAt(index);
            }

            // clear slot image
            currentImage.sprite = null;
            currentImage.color = new Color(0, 0.7f, 0.7f, 1);

            // shift remaining items left
            for (int i = index; i < potionCell.childCount - 1; i++)
            {
                currentImage = potionCell.GetChild(i).GetComponentInChildren<Image>();
                Image childImage = potionCell.GetChild(i + 1).GetComponentInChildren<Image>();
                if (childImage.sprite != null)
                {
                    currentImage.sprite = childImage.sprite;
                    currentImage.color = Color.white;
                    childImage.sprite = null;
                    childImage.color = new Color(0, 0.7f, 0.7f, 1);
                }
                else
                {
                    currentImage.color = new Color(0, 0.7f, 0.7f, 1);
                    break;
                }
            }
        }
    }

    public void RefreshInventoryUI()
    {
        // clear all cells first
        foreach (Transform cell in potionCell)
        {
            Image img = cell.GetComponentInChildren<Image>();
            if (img != null)
            {
                img.sprite = null;
                img.color = new Color(0, 0.7f, 0.7f, 1);
            }
        }

        // repopulate from playerData
        for (int i = 0; i < playerData.inventoryItems.Count; i++)
        {
            AddItemToInventoryGUI(playerData.inventoryItems[i].itemData.itemImage);
        }
    }
}
