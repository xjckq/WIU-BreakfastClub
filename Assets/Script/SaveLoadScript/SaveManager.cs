using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    public PlayerData playerData;
    public List<QuestData> allQuests;
    public QuestManager questManager;
    public InventorySystem inventorySystem;
    public ItemDatabase itemDatabase;
    public InventoryUI inventoryUI;
    public QuestUIManager questUIManager;

    public bool isLoadingSavedGame = false;

    private SaveData loadedSaveData;


    public void SaveGame()
    {
        SaveData data = new SaveData();

        // save the player stuff
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            data.playerPosition[0] = player.transform.position.x;
            data.playerPosition[1] = player.transform.position.y;
        }
        data.playerHealth = playerData.health;

        if (inventorySystem != null && playerData != null)
        {
            data.savedInventory.Clear();
            foreach (var itemInstance in playerData.inventoryItems)
            {
                data.savedInventory.Add(new ItemSaveData
                {
                    itemID = itemInstance.itemData.itemName
                });
                Debug.Log(data.savedInventory);
            }
        }

        // save the ongoing quests
        foreach (QuestData quest in questManager.activeQuests)
            data.activeQuestIDs.Add(quest.title);
        // save the completed quests
        foreach (QuestData quest in questManager.completedQuests)
            data.completedQuestIDs.Add(quest.title);
        // saves the progress of the active quests
        foreach (QuestProgress progress in questManager.questProgressList)
        {
            data.questProgresses.Add(new QuestProgressData
            {
                questID = progress.quest.title,
                progress = progress.progress
            });
        }

        // save the restored landmarks
        foreach (Landmark landmark in questManager.restoredLandmarks)
            data.restoredLandmarkIDs.Add(landmark.landmarkName);

        // save the states of the npcs [the dialogue state]
        NPC[] npcs = GameObject.FindObjectsByType<NPC>(FindObjectsSortMode.None);
        foreach (NPC npc in npcs)
        {
            data.npcStates.Add(new NPCSaveData
            {
                npcID = npc.npcData.npcName,
                state = npc.currentState,
                isFinished = npc.isFinished
            });
        }
        Debug.Log($"Saving active quests: {string.Join(", ", data.activeQuestIDs)}");
        Debug.Log($"Saving completed quests: {string.Join(", ", data.completedQuestIDs)}");

        SaveSystem.SaveGame(data);
    }

    public void LoadGame()
    {
        SaveData data = SaveSystem.LoadGame();
        if (data == null) return;

        isLoadingSavedGame = true;
        loadedSaveData = data;
        getAppliedData(data);
        isLoadingSavedGame = false;
    }

    public void getAppliedData(SaveData data)
    {
        // load the player
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null && isLoadingSavedGame)
        {
            player.transform.position = new Vector3(data.playerPosition[0], data.playerPosition[1], 0);
        }
        //playerData.health = data.playerHealth;

        if (playerData != null)
        {
            playerData.inventoryItems.Clear();

            foreach (ItemSaveData savedItem in data.savedInventory)
            {
                ItemData itemData = itemDatabase.GetItemByName(savedItem.itemID);
                if (itemData != null)
                {
                    ItemInstance newItem = new ItemInstance(itemData, itemData.theEffect);
                    playerData.inventoryItems.Add(newItem);
                }
                else
                {
                    Debug.LogWarning($"Item not found in database: {savedItem.itemID}");
                }
            }
        }

        // Then refresh the UI
        inventoryUI.RefreshInventoryUI();


        // reset the current quest data and larnmark stuff
        questManager.activeQuests.Clear();
        questManager.completedQuests.Clear();
        questManager.questProgressList.Clear();
        questManager.restoredLandmarks.Clear();

        // Load completed quests
        foreach (string questID in data.completedQuestIDs)
        {
            QuestData quest = allQuests.Find(q => q.title == questID);
            if (quest != null)
                questManager.completedQuests.Add(quest);
        }

        // Load active quests and progress
        foreach (QuestProgressData prog in data.questProgresses)
        {
            QuestData quest = allQuests.Find(q => q.title == prog.questID);
            if (quest != null)
            {
                questManager.activeQuests.Add(quest);
                questManager.questProgressList.Add(new QuestProgress
                {
                    quest = quest,
                    progress = prog.progress
                });
            }
        }

        // load the saved restried landmarks
        foreach (string lmID in data.restoredLandmarkIDs)
        {
            Landmark lm = questManager.restoredLandmarks.Find(l => l.landmarkName == lmID);
            if (lm != null)
                questManager.restoredLandmarks.Add(lm);
        }
        

        // load the saved npc dialogue state
        NPC[] npcs = GameObject.FindObjectsByType<NPC>(FindObjectsSortMode.None);
        foreach (NPCSaveData npcData in data.npcStates)
        {
            foreach (NPC npc in npcs)
            {
                if (npc.npcData.npcName == npcData.npcID)
                {
                    npc.currentState = npcData.state;
                    npc.isFinished = npcData.isFinished;
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