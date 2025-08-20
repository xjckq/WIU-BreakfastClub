using UnityEngine;

[CreateAssetMenu(fileName = "HealPotionEffect", menuName = "Scriptable Objects/HealPotionEffect")]
public class HealPotionEffect : ItemEffect
{
    public int healAmount;

    public override void Use(GameObject user)
    {
        var health=user.GetComponent<HealthSystem>();
        if (health != null)
        {
            health.Heal(healAmount);
        }
    }
}
