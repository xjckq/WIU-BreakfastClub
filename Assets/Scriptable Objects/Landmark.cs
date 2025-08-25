using UnityEngine;

[CreateAssetMenu(fileName = "Landmark", menuName = "Scriptable Objects/Landmark")]
public class Landmark : ScriptableObject
{
    public string landmarkName;
    public string landmarkSceneName;
    public Color restoredColor = Color.white; 
    public Color originalColor = Color.grey; 
}
