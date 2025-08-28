using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public PlayerData playerData;
    public QuestManager questManager;
    public InventorySystem inventorySystem;
    public ItemDatabase itemDatabase;
    public InventoryUI inventoryUI;
    public QuestUIManager questUIManager;
    public LandmarkDatabse landmarkDatabase;

    public List<QuestData> allQuests = new List<QuestData>();

    [HideInInspector] public bool isLoadingSavedGame = false;

    private SaveData loadedSaveData;

    void Start()
    {
        InitSceneObjects();
    }

    // Called by UI or SceneInitializer
    public void SaveGame()
    {
        SaveData data = new SaveData();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            data.playerPosition[0] = player.transform.position.x;
            data.playerPosition[1] = player.transform.position.y;
        }

        data.playerHealth = playerData.health;

        data.savedInventory.Clear();
        foreach (var itemInstance in playerData.inventoryItems)
        {
            data.savedInventory.Add(new ItemSaveData
            {
                itemID = itemInstance.itemData.itemName
            });
        }

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

        foreach (Landmark landmark in questManager.restoredLandmarks)
            data.restoredLandmarkIDs.Add(landmark.landmarkName);

        //NPC[] npcs = GameObject.FindObjectsByType<NPC>(FindObjectsSortMode.None);
        //foreach (NPC npc in npcs)
        //{
        //    data.npcStates.Add(new NPCSaveData
        //    {
        //        npcID = npc.npcData.npcName,
        //        state = npc.currentState,
        //        isFinished = npc.isFinished
        //    });
        //}

        if (NPCDialogueManager.Instance != null)
        {
            data.npcStates = NPCDialogueManager.Instance.GetDialogueSaveData();
        }
        Debug.Log($"Saved {data.npcStates.Count} NPC dialogue states.");

        SaveSystem.SaveGame(data);
        Debug.Log("Game saved.");
    }

    public void LoadGame()
    {
        SaveData data = SaveSystem.LoadGame();
        if (data == null)
        {
            Debug.LogWarning("No save data found.");
            return;
        }

        StartCoroutine(LoadAfterSceneReady(data));
    }

    IEnumerator LoadAfterSceneReady(SaveData data)
    {
        isLoadingSavedGame = true;

        // makes sure all scene objects r initialized
        yield return new WaitForEndOfFrame();

        InitSceneObjects();
        ApplySavedData(data);

        isLoadingSavedGame = false;
        Debug.Log("Game loaded.");
    }

    private void InitSceneObjects()
    {
        questManager = FindAnyObjectByType<QuestManager>();
        inventoryUI = FindAnyObjectByType<InventoryUI>();
        questUIManager = FindAnyObjectByType<QuestUIManager>();
        inventorySystem = FindAnyObjectByType<InventorySystem>();

        allQuests.Clear();
        QuestData[] quests = Resources.FindObjectsOfTypeAll<QuestData>();
        allQuests.AddRange(quests);
    }

    private void ApplySavedData(SaveData data)
    {
        // put player back to saved poisiton with saved health and inventory
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = new Vector3(data.playerPosition[0], data.playerPosition[1], 0);
        }

        playerData.health = data.playerHealth;
        playerData.inventoryItems.Clear();

        foreach (ItemSaveData savedItem in data.savedInventory)
        {
            ItemData itemData = itemDatabase.GetItemByName(savedItem.itemID);
            if (itemData != null)
            {
                ItemInstance newItem = new ItemInstance(itemData, itemData.theEffect);
                playerData.inventoryItems.Add(newItem);
            }
        }

        inventoryUI.RefreshInventoryUI(); // updates the ui n stuff

        // clear all the quest stuff so we can put the saved stuff back in
        questManager.activeQuests.Clear();
        questManager.completedQuests.Clear();
        questManager.questProgressList.Clear();
        questManager.restoredLandmarks.Clear();


        // restore the completed quests
        foreach (string questID in data.completedQuestIDs)
        {
            QuestData quest = null;
            foreach (var q in allQuests)
            {
                if (q.title == questID)
                {
                    quest = q;
                    break;
                }
            }

            if (quest != null && !questManager.completedQuests.Contains(quest))
            {
                questManager.completedQuests.Add(quest);
            }
        }

        // restore all the active quests
        foreach (string questID in data.activeQuestIDs)
        {
            QuestData quest = null;
            foreach (var q in allQuests)
            {
                if (q.title == questID)
                {
                    quest = q;
                    break;
                }
            }

            if (quest != null && !questManager.activeQuests.Contains(quest))
            {
                questManager.activeQuests.Add(quest);
            }
        }

        // restore all the quest progress
        foreach (QuestProgressData progressData in data.questProgresses)
        {
            QuestData quest = null;
            foreach (var q in allQuests)
            {
                if (q.title == progressData.questID)
                {
                    quest = q;
                    break;
                }
            }

            if (quest != null)
            {
                QuestProgress progress = new QuestProgress
                {
                    quest = quest,
                    progress = progressData.progress
                };
                questManager.questProgressList.Add(progress);
            }
        }

        foreach (string lmID in data.restoredLandmarkIDs)
        {
            Landmark landmark = null;

            foreach (var l in landmarkDatabase.allLandmarks)
            {
                if (l.landmarkName == lmID)
                {
                    landmark = l;
                    break;
                }
            }

            if (landmark != null)
            {
                Debug.Log($"Restoring landmark: {landmark.landmarkName}");
                questManager.restoredLandmarks.Add(landmark);
            }
        }


        // load the saved npcs states
        NPC[] npcs = GameObject.FindObjectsByType<NPC>(FindObjectsSortMode.None);
        foreach (NPCSaveData npcData in data.npcStates)
        {
            NPC npc = null;
            foreach (var n in npcs)
            {
                if (n.npcData.npcName == npcData.npcID)
                {
                    npc = n;
                    break;
                }
            }

            if (npc != null)
            {
                npc.currentState = npcData.state;
                npc.isFinished = npcData.isFinished;
            }

            // update dialogue manager
            if (NPCDialogueManager.Instance != null)
            {
                NPCDialogueManager.Instance.SetDialogueState(npcData.npcID, npcData.state, npcData.isFinished);
            }
        }
        Debug.Log($"Loading {data.npcStates.Count} NPC dialogue states.");
    }

    public void DeleteGameSave()
    {
        SaveSystem.DeleteSave();
        Debug.Log("Save deleted.");
    }
}


