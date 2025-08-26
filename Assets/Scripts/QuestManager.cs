using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class QuestProgress // to track quest progress
{
    public QuestData quest;
    public int progress;
    public bool canTurnInQuest = false;
}

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;
    public List<QuestData> allQuests;
    public List<QuestData> activeQuests;
    public List<QuestData> completedQuests;
    public List<Landmark> restoredLandmarks;
    public List<QuestProgress> questProgressList;

    public int maxActiveQuests = 3; 

    public int killEnemyCount;
    QuestProgress newProgress;

    public PlayerData playerData;

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
            return;
        }
    }


    public void StartQuest(QuestData quest)
    {
        // don't start if quest alr completed/is active/alr have 3 active quests
        if (completedQuests.Contains(quest) || activeQuests.Contains(quest) || activeQuests.Count == maxActiveQuests)
            return;
        
     
        activeQuests.Add(quest);   // add quest to active quest list
        newProgress = new QuestProgress(); // new progress tracking for the new quest
        newProgress.quest = quest;
        newProgress.progress = 0;
        questProgressList.Add(newProgress);

        Debug.Log("quest started: " + quest.title);
    }

    public void UpdateQuestProgress(QuestData quest, int amount = 1)
    {

        if (!activeQuests.Contains(quest)) // don't update if not active quest
            return;

        // find the progress for this quest
        for (int i = 0; i < questProgressList.Count; i++)
        {
            if (questProgressList[i].quest == quest)
            {
                questProgressList[i].progress += amount;

                // check if can turn in quest
                if (questProgressList[i].progress >= quest.requiredAmount)
                {
                    questProgressList[i].canTurnInQuest = true;
                    Debug.Log("can turn  in: " + quest.title);
                }
                return;
            }
        }
    }

    public int GetQuestProgress(QuestData quest)  // get current progress amount for quest
    {
        for (int i = 0; i < questProgressList.Count; i++) // loop through all the quest in progress
        {
            if (questProgressList[i].quest == quest)
            {
                return questProgressList[i].progress;
            }
        }
        return 0;
    }

    public void CompleteQuest(QuestData quest)
    {
        if (activeQuests.Contains(quest))
        {
            activeQuests.Remove(quest);
            completedQuests.Add(quest);
            RestoreLandmark(quest);

            // remove quest progress
            for (int i = questProgressList.Count - 1; i >= 0; i--)
            {
                if (questProgressList[i].quest == quest)
                {
                    questProgressList.RemoveAt(i);
                    break;
                }
            }
        }

        Debug.Log("quest completed: " + quest.title);
    }

    public bool IsQuestActive(QuestData quest)
    {
        return activeQuests.Contains(quest);
    }

    public bool IsQuestCompleted(QuestData quest)
    {
        return completedQuests.Contains(quest);
    }

    public bool IsQuestReadyToTurnIn(QuestData quest)
    {
        for (int i = 0; i < questProgressList.Count; i++)
        {
            if (questProgressList[i].quest == quest)
            {
                switch (quest.objectiveType)
                {
                    case QuestData.questObj.collectItems:
                        return HasRequiredItemsInInventory(quest);

                    case QuestData.questObj.talkToNPC:
                        return questProgressList[i].progress >= quest.requiredAmount;

                    default:
                        return questProgressList[i].canTurnInQuest;
                }
            }
        }
        return false;
    }

    private bool HasRequiredItemsInInventory(QuestData quest)
    {
        if (quest.requiredItem == null || playerData == null) 
            return false;

        int count = 0;
        foreach (ItemInstance item in playerData.inventoryItems)
        {
            if (item.itemData == quest.requiredItem)
            {
                count++;
                if (count >= quest.requiredAmount) 
                    return true; // found
            }
        }
        // not enough items in inventory
        return false; 
    }

    private void RestoreLandmark(QuestData quest)
    {
        if (quest.landmarkToRestore != null)
        {
            if (!restoredLandmarks.Contains(quest.landmarkToRestore))
            {
                GameObject landmarkObj = GameObject.Find(quest.landmarkToRestore.landmarkSceneName);

                if (landmarkObj != null)
                {
                    SpriteRenderer spriteRenderer = landmarkObj.GetComponent<SpriteRenderer>();
                    spriteRenderer.color = quest.landmarkToRestore.restoredColor;

                    ParticleSystem particle = landmarkObj.GetComponentInChildren<ParticleSystem>();
                    if (particle != null)
                    {
                        particle.Play();
                    }

                    restoredLandmarks.Add(quest.landmarkToRestore);

                    Debug.Log("landmark restored: " + quest.landmarkToRestore.landmarkName);

                }
                else
                {
                    Debug.LogWarning("can't find: " + quest.landmarkToRestore.landmarkSceneName) ;
                }
            }
        }
    }

    public void EnemyKilled()
    {
        for (int i = activeQuests.Count - 1; i >= 0; i--) // loops through all the active quests
        {
            if (activeQuests[i].objectiveType == QuestData.questObj.killEnemies) // check quest obj
                UpdateQuestProgress(activeQuests[i], 1); // update progress count
        }
    }

    public void ItemCollected()
    {
        for (int i = activeQuests.Count - 1; i >= 0; i--)
        {
            if (activeQuests[i].objectiveType == QuestData.questObj.collectItems)
                UpdateQuestProgress(activeQuests[i], 1);
        }
    }

    public void ItemCrafted(ItemData craftedItem)
    {
        for (int i = activeQuests.Count - 1; i >= 0; i--)
        {
            if (activeQuests[i].objectiveType == QuestData.questObj.craftItems)
            {
                if (activeQuests[i].requiredItem == craftedItem)
                {
                    UpdateQuestProgress(activeQuests[i], 1);
                }
            }
        }
    }

    public void MinigameCompleted()
    {
        for (int i = activeQuests.Count - 1; i >= 0; i--)
        {
            if (activeQuests[i].objectiveType == QuestData.questObj.completeMG)
                UpdateQuestProgress(activeQuests[i], 1);
        }
    }

    public void TalkedToNPC(NPC npc)
    {
        if (npc == null) return;

        for (int i = activeQuests.Count - 1; i >= 0; i--)
        {
            QuestData quest = activeQuests[i];

            if (quest.objectiveType == QuestData.questObj.talkToNPC && quest.npcToTalkTo == npc.npcData)
            {
                UpdateQuestProgress(quest, 1);
                return;
            }
        }
    }


}