using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestData", menuName = "Scriptable Objects/QuestData")]
public class QuestData : ScriptableObject
{
    public enum questObj
    {
        collectItems,
        craftItems,
        killEnemies,
        completeMG,
        talkToNPC
    }

    public enum EnemyType
    {
        None,
        Boar,
        Monkey
    }

    public enum MinigameType
    {
        None,
        Chicken,
        Kpod
    }

    public string title;
    public string desc;
    public questObj objectiveType;
    public int requiredAmount;
    public int moneyReward;
    public Landmark landmarkToRestore;
    public NPC questGiver;
    public NPCData npcToTalkTo;
    public List<NPCData> npcsToTalkTo;
    public ItemData requiredItem;
    public EnemyType targetEnemy;
    public MinigameType minigameType;

}
