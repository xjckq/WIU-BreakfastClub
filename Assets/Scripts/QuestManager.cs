using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;
    public List<QuestData> allQuests; 
    public List<QuestData> activeQuests;
    public List<QuestData> completedQuests;

    [SerializeField] TMP_Text questName;
    [SerializeField] TMP_Text questDesc;

    public int killEnemyCount;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartQuest(QuestData quest)
    {
        if (completedQuests.Contains(quest) == false && activeQuests.Contains(quest) == false)
            activeQuests.Add(quest);
        else
        {
            Debug.Log("quest have already been completed");
            return;
        }

        questName.text = quest.name;
        questDesc.text = quest.desc;
        if (quest.objectiveType == QuestData.questObj.killEnemies)
            questDesc.text += killEnemyCount + " / " + quest.requiredAmount;

        Debug.Log("quest started");
    }

    public void CompleteQuest(QuestData quest)
    {
        if (activeQuests.Contains(quest))
        {
            killEnemyCount = 0;
            activeQuests.Remove(quest);
            completedQuests.Add(quest);
            RestoreLandmark(quest);
        }

        questName.text = "Quest Name";
        questDesc.text = "There's no quests";
        Debug.Log("quest completed");
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
        // restore landmark

        Debug.Log("restored landmark");
    }
}
