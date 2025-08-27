using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "Scriptable Objects/WaveData")]
public class WaveData : ScriptableObject
{
    public GameObject[] zombiePrefabs;
    public int totalEnemiesToSpawn;
    public float spawnInterval;
}
