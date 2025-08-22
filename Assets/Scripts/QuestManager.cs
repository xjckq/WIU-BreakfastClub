using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class QuestProgress // to track quest progress
{
    public QuestData quest;
    public int progress;
}

[System.Serializable]
public class QuestUIPanel // to update quest ui
{
    public TMP_Text questName;
    public TMP_Text questDesc;
    public GameObject questPanel;
}

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;
    public List<QuestData> allQuests;
    public List<QuestData> activeQuests;
    public List<QuestData> completedQuests;
    public List<Landmark> restoredLandmarks;
    public List<QuestProgress> questProgressList;

    public List<QuestUIPanel> QuestUIPanels = new List<QuestUIPanel>(); 
    public int maxActiveQuests = 3; 

    public int killEnemyCount;
    QuestData currentQuest;
    string currentProgressTxt;
    QuestProgress newProgress;

    int killCount, itemCount, craftCount;

 
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

    void Update()
    {
        //UpdateQuestUI();
    }

    public void UpdateQuestUI()
    {
        // update for each quest
        for (int i = 0; i < QuestUIPanels.Count; i++)
        {
            if (i < activeQuests.Count && QuestUIPanels[i].questName != null && QuestUIPanels[i].questDesc != null)
            {
                currentQuest = activeQuests[i];
                QuestUIPanels[i].questName.text = currentQuest.title;

                currentProgressTxt = GetQuestProgressTxt(currentQuest);
                QuestUIPanels[i].questDesc.text = currentQuest.desc + currentProgressTxt;

                QuestUIPanels[i].questPanel.SetActive(true);
            }
            else
            {
                QuestUIPanels[i].questPanel.SetActive(false);
            }
        }
    }

    
    private string GetQuestProgressTxt(QuestData quest) // set progress txt based on the type of quest objective
    {
        switch (quest.objectiveType)
        {
            case QuestData.questObj.killEnemies:
                killCount = GetQuestProgress(quest);
                return " (" + killCount + "/" + quest.requiredAmount + ")";
            case QuestData.questObj.collectItems:
                itemCount = GetQuestProgress(quest);
                return " (" + itemCount + "/" + quest.requiredAmount + ")";

            case QuestData.questObj.craftItems:
                craftCount = GetQuestProgress(quest);
                return " (" + craftCount + "/" + quest.requiredAmount + ")";

            case QuestData.questObj.completeMG:
                return " (Complete minigame)";
            default:
                return "";
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

                // check if quest is complete
                if (questProgressList[i].progress >= quest.requiredAmount)
                {
                    CompleteQuest(quest);
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

    public void ItemCrafted()
    {
        for (int i = activeQuests.Count - 1; i >= 0; i--)
        {
            if (activeQuests[i].objectiveType == QuestData.questObj.craftItems)
                UpdateQuestProgress(activeQuests[i], 1);
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
}