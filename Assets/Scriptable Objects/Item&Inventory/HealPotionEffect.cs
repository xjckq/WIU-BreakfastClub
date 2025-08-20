using UnityEngine;

[CreateAssetMenu(fileName = "HealPotionEffect", menuName = "Scriptable Objects/HealPotionEffect")]
public class HealPotionEffect : ItemEffect
{
    public int healAmount;

    public override void Use(PlayerData playerData)
    {
        if (playerData != null)
        {
            playerData.Heal(healAmount);
            Debug.Log($"Healed {healAmount}, current HP = {playerData.health}/{playerData.maxHealth}");
        }
    }
}
