using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject itemsToSpawn;
    [SerializeField] private BoxCollider2D spawnAreaCollider;
    [SerializeField] private int totalNumberOfPrefabsToSpawn = 5;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private PlayerController playerGO;

    void Start()
    {
        SpawnPrefabs(totalNumberOfPrefabsToSpawn);
    }
    public void OnPrefabDestroyed()
    {
        SpawnPrefabs(1);
    }

    private void SpawnPrefabs(int count)
    {
        Bounds bounds = spawnAreaCollider.bounds;

        for (int i = 0; i < count; i++)
        {
            float randomX = Random.Range(bounds.min.x, bounds.max.x);
            float randomY = Random.Range(bounds.min.y, bounds.max.y);
            Vector3 randomSpawnPosition = new Vector3(randomX, randomY, 0f);
            GameObject spawnedPrefab = Instantiate(itemsToSpawn, randomSpawnPosition, Quaternion.identity);
            EnemyHealthSystem healthSystem = spawnedPrefab.GetComponent<EnemyHealthSystem>();
            NPCMovement npcMovement = spawnedPrefab.GetComponent<NPCMovement>();
            if (healthSystem != null)
            {
                healthSystem.OnDied.AddListener(OnPrefabDestroyed);
            }
            if (npcMovement != null)
            {
                npcMovement.SetPlayer(playerTransform);
            }
            if (npcMovement != null)
            {
                npcMovement.SetPlayerHealth(playerGO);
            }
        }
    }
}
