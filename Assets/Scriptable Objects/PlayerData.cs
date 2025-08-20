using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    public int health;
    public int maxHealth;
    public int money;

    public void AddMoney(int amt)
    {
        money += amt;
    }

    public void TakeDmg(int dmg)
    {
        health -= dmg;
        if (health < 0)
            health = 0;
    }

    public void Heal(int amt)
    {

        health += amt;
        if (health > maxHealth)
            health = maxHealth;
    }

    public void ResetData()
    {
        health = 100;
        money = 0;
        maxHealth = 100;
    }

}
