using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventoryUI : MonoBehaviour
{
    public PlayerData playerData;
    public InventorySystem inventorySystem;

    // foods
    public Transform foodCell;
    public bool buffFoodInUse = false;
    public TMP_Text foodInUseText;
    public TMP_Text HPfullText;
    private Coroutine buffFoodCoroutine;

    public AudioSource audioSource;
    public AudioClip useItemSound;

    void Awake()
    {
        RefreshInventoryUI();
        ResetBuffs();
    }

    public void ResetBuffs()
    {
        // stop food buff coroutine
        if (buffFoodCoroutine != null)
        {
            StopCoroutine(buffFoodCoroutine);
            buffFoodCoroutine = null;
        }

        buffFoodInUse = false;
        playerData.buffFoodActive = false;
        playerData.buffFoodRemainingTime = 0f;
        foodInUseText.gameObject.SetActive(false);
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
        foreach (Transform cell in foodCell)
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
        if (playerData != null){}
            playerData.inventoryItems.Add(item);
    }

    public void RemoveItemFromPlayerData(int index)
    {
        if (playerData != null && index >= 0 && index < playerData.inventoryItems.Count)
            playerData.inventoryItems.RemoveAt(index);
    }

    private IEnumerator BuffFoodTimer(float duration)
    {
        float remaining = duration;
        while (remaining > 0)
        {
            playerData.buffFoodRemainingTime = remaining;
            remaining -= Time.deltaTime;
            yield return null;
        }

        buffFoodInUse = false;
        playerData.buffFoodActive = false;
        playerData.buffFoodRemainingTime = 0;

        foodInUseText.gameObject.SetActive(false);
    }

    public void UseItem(Button selectedItemButton)
    {
        if (playerData == null) return;

        Image currentImage = selectedItemButton.GetComponentInChildren<Image>();
        if (currentImage.sprite != null)
        {
            // buff food check
            if (currentImage.sprite.name == "wantonnoodle_0")
            {
                // if in use, refresh timer
                if (buffFoodInUse)
                {
                    if (buffFoodCoroutine != null)
                    {
                        StopCoroutine(buffFoodCoroutine);
                    }
                    playerData.buffFoodRemainingTime += 10f;
                    buffFoodCoroutine = StartCoroutine(BuffFoodTimer(playerData.buffFoodRemainingTime));
                }
                else
                {
                    buffFoodInUse = true;
                    playerData.buffFoodActive = true;
                    playerData.buffFoodRemainingTime = 10f;

                    foodInUseText.gameObject.SetActive(true);
                    buffFoodCoroutine = StartCoroutine(BuffFoodTimer(playerData.buffFoodRemainingTime));
                }
            }

            // heal food check
            if (currentImage.sprite.name == "gorengpisang_0")
            {
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

                // only use if the item has an effect
                if (item.itemEffect != null)
                {
                    item.itemEffect.Use(playerData);
                    audioSource.PlayOneShot(useItemSound);
                    playerData.inventoryItems.RemoveAt(index);

                    // clear slot image
                    currentImage.sprite = null;
                    currentImage.color = new Color(0, 0.7f, 0.7f, 0);

                    ShiftCells(index);
                }
                else
                {
                    Debug.Log("item has no effect: " + item.itemData.itemName);
                    return;
                }
            }

        }
    }

    public void DropItem(Button selectedItemButton)
    {
        if (playerData == null) return;
        int index = int.Parse(selectedItemButton.name[selectedItemButton.name.Length - 1].ToString()) - 1;

        if (index < 0 || index >= playerData.inventoryItems.Count) return;
        ItemInstance droppedItem = playerData.inventoryItems[index];

        // remove from player data
        playerData.inventoryItems.RemoveAt(index);

        // clear slot image
        Image currentImage = foodCell.GetChild(index).GetComponentInChildren<Image>();
        currentImage.sprite = null;
        currentImage.color = new Color(0, 0.7f, 0.7f, 0);

        ShiftCells(index);

        // drop item next to player
        GameObject droppedObj = new GameObject(droppedItem.itemData.itemName);
        droppedObj.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position + Vector3.right;

        // set scale and add components
        droppedObj.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        var sr = droppedObj.AddComponent<SpriteRenderer>();
        sr.sortingOrder = 5;
        sr.sprite = droppedItem.itemData.itemImage;
        var col = droppedObj.AddComponent<CircleCollider2D>();
        col.isTrigger = true;
        var itemComp = droppedObj.AddComponent<Item>();
        itemComp.item = droppedItem;

        Debug.Log($"dropped {droppedItem.itemData.itemName}");
    }

    // shift remaining items
    private void ShiftCells(int startIndex)
    {
        for (int i = startIndex; i < foodCell.childCount - 1; i++)
        {
            Image thisImg = foodCell.GetChild(i).GetComponentInChildren<Image>();
            Image nextImg = foodCell.GetChild(i + 1).GetComponentInChildren<Image>();

            if (nextImg.sprite != null)
            {
                thisImg.sprite = nextImg.sprite;
                thisImg.color = Color.white;
                nextImg.sprite = null;
                nextImg.color = new Color(0, 0.7f, 0.7f, 0);
            }
            else
            {
                thisImg.sprite = null;
                thisImg.color = new Color(0, 0.7f, 0.7f, 0);
                break;
            }
        }
    }

    public void RefreshInventoryUI()
    {
        // clear all cells first
        foreach (Transform cell in foodCell)
        {
            Image img = cell.GetComponentInChildren<Image>();
            if (img != null)
            {
                img.sprite = null;
                img.color = new Color(0, 0.7f, 0.7f, 0);
            }
        }

        // repopulate from playerData
        for (int i = 0; i < playerData.inventoryItems.Count; i++)
        {
            AddItemToInventoryGUI(playerData.inventoryItems[i].itemData.itemImage);
        }
    }
}
