using UnityEngine;

[CreateAssetMenu(fileName = "QuestData", menuName = "Scriptable Objects/QuestData")]
public class QuestData : ScriptableObject
{
    public enum questObj
    {
        collectItems,
        craftItems,
        killEnemies,
        completeMG
    }

    public string title;
    public string desc;
    public questObj objectiveType;
    public int requiredAmount;
    public int moneyReward;
    public Landmark landmarkToRestore;
    //public ItemData requiredItem;
    //public string Item[] itemReward;

}
