using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthSystem : MonoBehaviour
{
    public PlayerData playerData;

    // stats
    public float MaxHealth = 200f;
    public float Health { get; set; }

    // UI
    public Image HPbar; 
    public TMP_Text HPText;
    public GameObject blood;

    // death
    public GameObject gameOverPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (playerData != null)
        {
            if (playerData.currentHP <= 0)
            {
                playerData.currentHP = MaxHealth;
            }
            Debug.Log(playerData.currentHP);
            // sync HP to playerData
            Health = playerData.currentHP;
        }

        UpdateHealth();
    }

    private void Awake()
    {
        PlayerData Instance = playerData; 
    }

    public void ReduceHealth(float amount)
    {
        if (playerData == null) return;

        playerData.currentHP -= amount;
        if (playerData.currentHP < 0)
        {
            playerData.currentHP = 0;
        }

        // blood particles
        GameObject bloodInstance = Instantiate(blood, transform.position, Quaternion.identity);
        Destroy(bloodInstance, 1f);

        Health = playerData.currentHP;
        UpdateHealth();

        if (Health <= 0)
        {
            var player = GetComponent<PlayerController>();
            if (player != null)
                player.Die();
        }
    }
    public void Heal(float amount)
    {
        if (playerData == null) return;

        playerData.currentHP += amount;
        if (playerData.currentHP > MaxHealth) playerData.currentHP = MaxHealth;

        Health = playerData.currentHP;
        UpdateHealth();
    }

    public void UpdateHealth()
    {
        if (HPbar != null)
            HPbar.fillAmount = playerData.currentHP / MaxHealth;

        if (HPText != null)
            HPText.text = $"HP {Mathf.RoundToInt(playerData.currentHP)} / {Mathf.RoundToInt(MaxHealth)}";
    }
}
