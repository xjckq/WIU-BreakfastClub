using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    public PlayerData playerData;
    public List<QuestData> allQuests;
    public QuestManager questManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveGame()
    {
        SaveData data = new SaveData();

        // Save player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            data.playerPosition[0] = player.transform.position.x;
            data.playerPosition[1] = player.transform.position.y;
        }

        data.playerHealth = playerData.health;

        // Save quests
        foreach (QuestData quest in questManager.activeQuests)
            data.activeQuestIDs.Add(quest.title);

        foreach (QuestData quest in questManager.completedQuests)
            data.completedQuestIDs.Add(quest.title);

        foreach (QuestProgress progress in questManager.questProgressList)
        {
            data.questProgresses.Add(new QuestProgressData
            {
                questID = progress.quest.title,
                progress = progress.progress
            });
        }

        // Save landmarks
        foreach (Landmark landmark in questManager.restoredLandmarks)
            data.restoredLandmarkIDs.Add(landmark.landmarkName);

        // Save NPCs
        NPC[] npcs = GameObject.FindObjectsByType<NPC>(FindObjectsSortMode.None);
        foreach (NPC npc in npcs)
        {
            data.npcStates.Add(new NPCSaveData
            {
                npcID = npc.npcData.npcName,
                state = npc.currentState
            });
        }

        SaveSystem.SaveGame(data);
    }

    public void LoadGame()
    {
        SaveData data = SaveSystem.LoadGame();
        if (data == null) return;

        // Load player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            player.transform.position = new Vector3(data.playerPosition[0], data.playerPosition[1], 0);

        playerData.health = data.playerHealth;

        // Reset quest data
        questManager.activeQuests.Clear();
        questManager.completedQuests.Clear();
        questManager.questProgressList.Clear();
        questManager.restoredLandmarks.Clear();

        // Load quests
        foreach (string questID in data.activeQuestIDs)
        {
            QuestData quest = allQuests.Find(q => q.title == questID);
            if (quest != null)
                questManager.activeQuests.Add(quest);
        }

        foreach (string questID in data.completedQuestIDs)
        {
            QuestData quest = allQuests.Find(q => q.title == questID);
            if (quest != null)
                questManager.completedQuests.Add(quest);
        }

        foreach (QuestProgressData prog in data.questProgresses)
        {
            QuestData quest = allQuests.Find(q => q.title == prog.questID);
            if (quest != null)
            {
                questManager.questProgressList.Add(new QuestProgress
                {
                    quest = quest,
                    progress = prog.progress
                });
            }
        }

        // Load landmarks
        foreach (string lmID in data.restoredLandmarkIDs)
        {
            Landmark lm = questManager.restoredLandmarks.Find(l => l.landmarkName == lmID);
            if (lm != null)
                questManager.restoredLandmarks.Add(lm);
        }

        // Load NPC dialogue states
        NPC[] npcs = GameObject.FindObjectsByType<NPC>(FindObjectsSortMode.None);
        foreach (NPCSaveData npcData in data.npcStates)
        {
            foreach (NPC npc in npcs)
            {
                if (npc.npcData.npcName == npcData.npcID)
                {
                    npc.currentState = npcData.state;
                    break;
                }
            }
        }

        Debug.Log("Game loaded successfully.");
    }

    public void DeleteGameSave()
    {
        SaveSystem.DeleteSave();
    }
}