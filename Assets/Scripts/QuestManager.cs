using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;
    public List<QuestData> allQuests; 
    public List<QuestData> activeQuests;
    public List<QuestData> completedQuests;

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

        Debug.Log("quest started");
    }

    public void CompleteQuest(QuestData quest)
    {
        if (activeQuests.Contains(quest))
        {
            activeQuests.Remove(quest);
            completedQuests.Add(quest);
            RestoreLandmark(quest);
        }
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
