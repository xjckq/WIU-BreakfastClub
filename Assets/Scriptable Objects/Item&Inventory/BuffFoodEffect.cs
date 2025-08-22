using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "BuffFoodEffect", menuName = "Scriptable Objects/BuffFoodEffect")]
public class BuffFoodEffect : ItemEffect
{
    public override void Use(PlayerData playerData)
    {
        if (playerData == null) return;

        // use buff food
        playerData.currentSpeed += 2;

        if (playerData.buffFoodActive)
        { 
            // if already in use, refresh timer
            playerData.buffFoodRemainingTime += duration;
            Debug.Log($"Buff refreshed, new remaining time: {playerData.buffFoodRemainingTime:F1}s");
        }
        else
        {
            // buff speed 
            playerData.buffFoodActive = true;
            playerData.buffFoodRemainingTime = duration;
            Debug.Log($"Buff applied for {duration:F1}s");
        }
    }
}