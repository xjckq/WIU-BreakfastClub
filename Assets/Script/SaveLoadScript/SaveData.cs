using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public int playerHealth;
    public float[] playerPosition = new float[2];

    public List<string> activeQuestIDs = new List<string>();
    public List<string> completedQuestIDs = new List<string>();
    public List<QuestProgressData> questProgresses = new List<QuestProgressData>();
    public List<string> restoredLandmarkIDs = new List<string>();
    public List<NPCSaveData> npcStates = new List<NPCSaveData>();
}

[System.Serializable]
public class NPCSaveData
{
    public string npcID;
    public DialogueState state;
}

[Serializable]
public class QuestProgressData
{
    public string questID;
    public int progress;
}