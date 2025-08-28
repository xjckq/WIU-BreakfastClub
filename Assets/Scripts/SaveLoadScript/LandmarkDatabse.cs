using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LandmarkDatabse", menuName = "Scriptable Objects/LandmarkDatabse")]
public class LandmarkDatabse : ScriptableObject
{
    public List<Landmark> allLandmarks;
}
