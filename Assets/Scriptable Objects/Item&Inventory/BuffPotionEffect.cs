using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "BuffPotionEffect", menuName = "Scriptable Objects/BuffPotionEffect")]
public class BuffPotionEffect : ItemEffect
{
    public override void Use(PlayerData playerData)
    {
        if (playerData == null) return;

        // use buff potion
        playerData.currentSpeed += 2;

        if (playerData.buffPotionActive)
        { 
            // if already in use, refresh timer
            playerData.buffPotionRemainingTime += duration;
            Debug.Log($"Buff refreshed, new remaining time: {playerData.buffPotionRemainingTime:F1}s");
        }
        else
        {
            // buff speed 
            playerData.buffPotionActive = true;
            playerData.buffPotionRemainingTime = duration;
            Debug.Log($"Buff applied for {duration:F1}s");
        }
    }
}