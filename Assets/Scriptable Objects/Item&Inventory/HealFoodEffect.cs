using UnityEngine;

[CreateAssetMenu(fileName = "HealFoodEffect", menuName = "Scriptable Objects/HealFoodEffect")]
public class HealFoodEffect : ItemEffect
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
