using UnityEngine;

[CreateAssetMenu(fileName = "HealFoodEffect", menuName = "Scriptable Objects/HealFoodEffect")]
public class HealFoodEffect : ItemEffect
{
    public int healAmount;

    public override void Use(PlayerData playerData)
    {
        var player = GameObject.FindFirstObjectByType<PlayerController>();
        if (player != null)
        {
            player.Heal(healAmount);
            Debug.Log($"Healed {healAmount}, current HP = {player.playerData.health}/{player.playerData.maxHealth}");
        }
    }
}
