using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static QuestData;

[System.Serializable]
public class QuestProgress // to track quest progress
{
    public QuestData quest;
    public int progress;
    public bool canTurnInQuest = false;

    public List<NPCData> talkedToNPCs = new List<NPCData>();
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

    bool NPCprogressUpdated;
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
                if (quest.objectiveType == QuestData.questObj.talkToNPC && quest.npcsToTalkTo != null && quest.npcsToTalkTo.Count > 0)
                {
                    return questProgressList[i].talkedToNPCs.Count;
                }
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

            if(quest.moneyReward > 0)
            {
                playerData.money += quest.moneyReward;
                Debug.Log("money received: " + quest.moneyReward);
            }

            if (quest.objectiveType == QuestData.questObj.collectItems && quest.requiredItem != null || quest.objectiveType == QuestData.questObj.craftItems && quest.requiredItem != null)
            {
                removeItemFromInventory(quest.requiredItem, quest.requiredAmount);
                // refresh inventory UI
                var inventory = FindAnyObjectByType<InventorySystem>();
                if (inventory != null)
                    inventory.inventoryUI.RefreshInventoryUI();
            }


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

    private void removeItemFromInventory(ItemData itemToRemove, int amountToRemove)
    {
        int removedCount = 0;

        for (int i = playerData.inventoryItems.Count - 1; i >= 0 && removedCount < amountToRemove; i--)
        {
            if (playerData.inventoryItems[i].itemData == itemToRemove)
            {
                playerData.inventoryItems.RemoveAt(i);
                removedCount++;
                Debug.Log("removed " + itemToRemove.itemName + " from inventory");
            }
        }
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
                        if (quest.npcsToTalkTo != null && quest.npcsToTalkTo.Count > 0)
                        {
                            return questProgressList[i].talkedToNPCs.Count >= quest.npcsToTalkTo.Count;
                        }
                        else
                        {
                            return questProgressList[i].progress >= quest.requiredAmount;
                        }

                    default:
                        return questProgressList[i].canTurnInQuest;
                }
            }
        }
        return false;
    }

    private bool HasRequiredItemsInInventory(QuestData quest)
    {
        for (int i = 0; i < questProgressList.Count; i++)
        {
            if (questProgressList[i].quest == quest)
            {
                return questProgressList[i].canTurnInQuest;
            }
        }
        return false;
    }

    public void ItemCollected(ItemData collectedItem)
    {
        for (int i = activeQuests.Count - 1; i >= 0; i--)
        {
            QuestData quest = activeQuests[i];

            if (quest.objectiveType == QuestData.questObj.collectItems && quest.requiredItem == collectedItem)
            {
                UpdateCollectionQuestProgress(quest);
            }
        }
    }

    private void UpdateCollectionQuestProgress(QuestData quest)
    {
        if (quest.requiredItem == null || playerData == null)
            return;

        // how many of the required item the player has
        int currentCount = 0;
        foreach (ItemInstance item in playerData.inventoryItems)
        {
            if (item.itemData == quest.requiredItem)
            {
                currentCount++;
            }
        }

        // find and update the quest progress
        for (int i = 0; i < questProgressList.Count; i++)
        {
            if (questProgressList[i].quest == quest)
            {
                if (currentCount >= quest.requiredAmount)
                {
                    questProgressList[i].progress = quest.requiredAmount;
                    questProgressList[i].canTurnInQuest = true;
                    Debug.Log("can turn in: " + quest.title);
                }
                else
                {
                    questProgressList[i].progress = currentCount;
                    questProgressList[i].canTurnInQuest = false;
                }
                break;
            }
        }
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

    public void EnemyKilled(EnemyType enemyType)
    {
        for (int i = activeQuests.Count - 1; i >= 0; i--) // loops through all the active quests
        {
            QuestData quest = activeQuests[i];

            if (quest.objectiveType == QuestData.questObj.killEnemies)
            {
                if  (quest.targetEnemy == enemyType)
                {
                    UpdateQuestProgress(quest, 1);
                }
            }
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

    public void MinigameCompleted(QuestData.MinigameType type)
    {
        for (int i = activeQuests.Count - 1; i >= 0; i--)
        {
            QuestData quest = activeQuests[i];

            if (quest.objectiveType == QuestData.questObj.completeMG && quest.minigameType == type)
            {
                UpdateQuestProgress(quest, 1);

                if (IsQuestReadyToTurnIn(quest))
                {
                    Debug.Log("quest can be turned in: " + quest.title);
                }
            }
        }
    }
    public void TalkedToNPC(NPC npc)
    {
        if (npc == null) return;

        NPCprogressUpdated = false;
        for (int i = activeQuests.Count - 1; i >= 0; i--)
        {
            QuestData quest = activeQuests[i];

            if (quest.objectiveType == QuestData.questObj.talkToNPC)
            {
                QuestProgress progress = questProgressList.Find(qp => qp.quest == quest);
                if (progress == null) continue;

                // for quest that requires u to only talk to 1 npc
                if (quest.npcToTalkTo != null && quest.npcToTalkTo == npc.npcData)
                {
                    UpdateQuestProgress(quest, 1);
                    NPCprogressUpdated = true;
                    continue;
                }

                // for quest that requires u to talk to multiple npcs
                if (quest.npcsToTalkTo != null && quest.npcsToTalkTo.Count > 0)
                {
                    if (quest.npcsToTalkTo.Contains(npc.npcData) && !progress.talkedToNPCs.Contains(npc.npcData))
                    {
                        progress.talkedToNPCs.Add(npc.npcData);
                        progress.progress = progress.talkedToNPCs.Count;

                        if (progress.progress >= quest.npcsToTalkTo.Count)
                        {
                            progress.canTurnInQuest = true;
                            Debug.Log("all NPCs talked to for quest: " + quest.title);
                        }
                        NPCprogressUpdated = true;
                        continue;
                    }
                }
            }
        }

    }



}