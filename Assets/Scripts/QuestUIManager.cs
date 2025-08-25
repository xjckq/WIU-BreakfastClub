using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestUIManager : MonoBehaviour
{

    public List<TMP_Text> questNames;   
    public List<TMP_Text> questDescs;    
    public List<GameObject> questPanels;
    int progress;

    private void Update()
    {
        UpdateQuestUI();
    }

    public void UpdateQuestUI()
    {

        for (int i = 0; i < questPanels.Count; i++)
        {
            if (i < QuestManager.Instance.activeQuests.Count) 
            {
                QuestData currentQuest = QuestManager.Instance.activeQuests[i];

                if (questNames[i] != null)
                    questNames[i].text = currentQuest.title;

                if (questDescs[i] != null)
                    questDescs[i].text = currentQuest.desc + GetQuestProgressTxt(currentQuest);

                questPanels[i].SetActive(true);
            }
            else
            {
                questPanels[i].SetActive(false);
            }
        }
    }

    private string GetQuestProgressTxt(QuestData quest)
    {
       progress = QuestManager.Instance.GetQuestProgress(quest);

        switch (quest.objectiveType)
        {
            case QuestData.questObj.killEnemies:
                return $" ({progress}/{quest.requiredAmount})";
            case QuestData.questObj.collectItems:
                return $" ({progress}/{quest.requiredAmount})";
            case QuestData.questObj.craftItems:
                return $" ({progress}/{quest.requiredAmount})";
            case QuestData.questObj.completeMG:
                return " (Complete minigame)";
            default:
                return "";
        }
    }
}
