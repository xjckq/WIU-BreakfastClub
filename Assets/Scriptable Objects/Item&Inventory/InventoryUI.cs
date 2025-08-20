using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventoryUI : MonoBehaviour
{
    public PlayerData playerData;
    public InventorySystem inventorySystem;
    public HealthSystem healthSystem;

    // potions
    public Transform potionCell;
    public bool buffPotionInUse = false;
    public TMP_Text potionInUseText;
    public TMP_Text HPfullText;

    // status
    public Image buffStatus;

    void Start()
    {
        // populate from gamedata
        if (gameData != null && gameData.inventoryItems.Count > 0)
        {
            foreach (var item in gameData.inventoryItems)
                AddItemToInventoryGUI(item.itemData.itemImage);
        }

        // restore buff if active
        if (gameData != null && gameData.buffPotionActive)
        {
            buffPotionInUse = true;
            potionInUseText.gameObject.SetActive(true);
            buffStatus.gameObject.SetActive(true);

            // restart timer coroutine with remaining time
            StartCoroutine(BuffPotionTimer(gameData.buffPotionRemainingTime));
        }
    }

    void Update()
    {
        // hide HP full text if HP not full
        if (HPfullText.gameObject.activeSelf && healthSystem.Health < healthSystem.MaxHealth)
        {
            HPfullText.gameObject.SetActive(false);
        }
    }

    public void AddItemToInventoryGUI(Sprite newItem)
    {
        foreach (Transform cell in potionCell)
        {
            Debug.Log(cell);
            Image itemImage = cell.GetComponentInChildren<Image>();

            if (itemImage != null && itemImage.sprite == null)
            {
                Debug.Log("item collected");
                itemImage.sprite = newItem;
                itemImage.enabled = true;
                itemImage.color = Color.white;
                break; 
            }
        }
    }

    public void AddItemToGameData(ItemInstance item)
    {
        if (gameData != null)
            playerData.inventoryItems.Add(item);
    }

    public void RemoveItemFromplayerData(int index)
    {
        if (playerData != null && index >= 0 && index < playerData.inventoryItems.Count)
            playerData.inventoryItems.RemoveAt(index);
    }

    private IEnumerator BuffPotionTimer(float duration)
    {
        Debug.Log("buff timer started");
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
        buffStatus.gameObject.SetActive(false);
        Debug.Log("buff effect expired.");
    }


    public void UseItem(Button selectedItemButton)
    {
        if (playerData == null) return;

        Image currentImage = selectedItemButton.GetComponentInChildren<Image>();
        if (currentImage.sprite != null)
        {
            // buff potion check
            if(currentImage.sprite.name == "SprTiles_56")
            {
                if (buffPotionInUse)
                {
                    // if buff potion alr in use, dont use again
                    Debug.Log("buff potion already used");
                    return;
                }
                else
                {
                    // start timer fo buff potion
                    buffPotionInUse = true;
                    playerData.buffPotionActive = true;
                    playerData.buffPotionRemainingTime = 10f;

                    potionInUseText.gameObject.SetActive(true);
                    buffStatus.gameObject.SetActive(true);
                    StartCoroutine(BuffPotionTimer(10));
                }
            }

            // heal potion check
            if (currentImage.sprite.name == "SprTiles_57")
            {
                if (healthSystem.playerData.currentHP >= healthSystem.MaxHealth)
                {
                    // if HP full, dont use heal potion
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
                item.itemEffect.Use(healthSystem.gameObject);
                playerData.inventoryItems.RemoveAt(index);
            }

            Image itemImage = selectedItemButton.GetComponentInChildren<Image>();
            itemImage.sprite = null;
            itemImage.color = new Color(0.7176471f, 0.4431373f, 0.4431373f, 1f);
            Debug.Log(selectedItemButton.name + " was pressed");

            for (int i = index; i < potionCell.childCount - 1; i++)
            {
                currentImage = potionCell.GetChild(i).GetComponentInChildren<Image>();
                Image childImage = potionCell.GetChild(i + 1).GetComponentInChildren<Image>();
                if (childImage.sprite != null)
                {
                    currentImage.sprite = childImage.sprite;
                    currentImage.color = Color.white;
                    childImage.sprite = null;
                    childImage.color = new Color(0.7176471f, 0.4431373f, 0.4431373f, 1f);
                }
                else
                {
                    currentImage.color = new Color(0.7176471f, 0.4431373f, 0.4431373f, 1f);
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
                img.color = new Color(0.7176471f, 0.4431373f, 0.4431373f, 1f);
            }
        }

        // repopulate from playerData
        for (int i = 0; i < playerData.inventoryItems.Count; i++)
        {
            AddItemToInventoryGUI(playerData.inventoryItems[i].itemData.itemImage);
        }
    }

}