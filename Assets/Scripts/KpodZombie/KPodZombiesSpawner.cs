using UnityEngine;
using System.Collections;
public class KPodZombiesSpawner : MonoBehaviour
{
    private BoxCollider2D spawnAreaCollider;
    void Awake()
    {
        spawnAreaCollider = GetComponent<BoxCollider2D>();
    }

    public void SpawnEnemyAtRandomPoint(GameObject enemyPrefab)
    {
        if (enemyPrefab != null && spawnAreaCollider != null)
        {
            Vector2 spawnPosition = GetRandomPointInCollider();
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }
    private Vector2 GetRandomPointInCollider()
    {
        Bounds bounds = spawnAreaCollider.bounds;
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);

        return new Vector2(randomX, randomY);
    }
}
