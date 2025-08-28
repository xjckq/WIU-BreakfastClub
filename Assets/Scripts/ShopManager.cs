using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ShopManager : MonoBehaviour
{
    public ShopItems shopItems;
    public ShopItemSlot[] shopSlots;

    private float _refreshTimer;
    public TextMeshProUGUI refreshTimerText;

    void Start()
    {
        RandomiseShopItems();
        _refreshTimer = shopItems.shopRefreshTime;
    }
    void Update()
    {
        if (refreshTimerText != null)
        {
            int minutes = Mathf.FloorToInt(_refreshTimer / 60f);
            int seconds = Mathf.FloorToInt(_refreshTimer % 60f);
            refreshTimerText.text = $"Refresh in: {minutes:00}:{seconds:00}";
        }
        _refreshTimer -= Time.deltaTime;
        if (_refreshTimer <= 0)
        {
            RandomiseShopItems();
            _refreshTimer = shopItems.shopRefreshTime;
        }
    }
    void RandomiseShopItems()
    {
        for (int i = 0; i < shopSlots.Length; i++)
        {
            int randomIndex = Random.Range(0, shopItems.allItems.Count);
            ItemData randomItem = shopItems.allItems[randomIndex];
            shopSlots[i].InitialiseItem(randomItem);
        }
    }
}
