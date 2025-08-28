using UnityEngine;

public class SpawnOnLoad : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;

    private void Awake()
    {
        if (playerData != null && playerData.hasCustomSpawn)
        {
            transform.position = playerData.spawnPosition;
            playerData.hasCustomSpawn = false;
        }
    }
}
