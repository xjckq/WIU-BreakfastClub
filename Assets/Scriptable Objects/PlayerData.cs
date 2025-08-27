using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    // stats
    public int health;
    public int maxHealth;
    public int money;

    // movement
    public int speed;
    public int currentSpeed;

    // spd buff
    public bool buffFoodActive = false;
    public float buffFoodRemainingTime;

    // inventory
    public List<ItemInstance> inventoryItems = new List<ItemInstance>();
    public int maxInventorySize = 10;

    // player spawn
    public Vector3 spawnPosition;
    public bool hasCustomSpawn;

    public void AddMoney(int amt)
    {
        money += amt;
    }

    public void ResetData()
    {
        health = 100;
        money = 0;
        maxHealth = 100;

        currentSpeed = speed;
        buffFoodActive = false;
        buffFoodRemainingTime = 0f;

        inventoryItems.Clear();

        hasCustomSpawn = false;
        spawnPosition = Vector3.zero;
    }
}
