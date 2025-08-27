using UnityEngine;

[CreateAssetMenu(fileName = "ZombieData", menuName = "Scriptable Objects/ZombieData")]
public class ZombieData : ScriptableObject
{
    public int health;
    public int moveSpeed;
    public int damage;
    //public GameObject zombiePrefab;
}
