using UnityEngine;

[CreateAssetMenu(fileName = "Landmark", menuName = "Scriptable Objects/Landmark")]
public class Landmark : ScriptableObject
{
    public string landmarkName;
    public GameObject gameObject; 
    public Sprite fadedSprite;
    public Sprite restoredSprite;
}
