using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public int playerHealth;
    public float[] playerPosition = new float[2];

    public string currentSceneName; // save the scene where player saved

    public List<string> activeQuestIDs = new List<string>(); // get the quest IDs
    public List<string> completedQuestIDs = new List<string>(); // get the IDs of completed quests
    public List<QuestProgressData> questProgresses = new List<QuestProgressData>(); // get the progress of ongoing quests
    public List<string> restoredLandmarkIDs = new List<string>(); // get the state of activated landmarks
    public List<NPCSaveData> npcStates = new List<NPCSaveData>(); // get the npcStates [their dialogue]
}

[System.Serializable]
public class NPCSaveData
{
    public string npcID;
    public DialogueState state;
    public bool isFinished;
}

[Serializable]
public class QuestProgressData
{
    public string questID;
    public int progress;
}