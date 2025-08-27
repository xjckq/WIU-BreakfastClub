using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

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
                {
                    if (QuestManager.Instance.IsQuestReadyToTurnIn(currentQuest))
                    {
                        string npcName;
                        if (currentQuest.questGiver != null)
                            npcName = currentQuest.questGiver.npcData.npcName;
                        else
                            npcName = "NPC";

                        questDescs[i].text = "Go back to talk to " + npcName;
                    }
                    else
                    {
                        questDescs[i].text = currentQuest.desc + GetQuestProgressTxt(currentQuest);
                    }
                }

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
            case QuestData.questObj.collectItems:
            case QuestData.questObj.craftItems:
                return $" ({progress}/{quest.requiredAmount})";

            case QuestData.questObj.completeMG:
                return " (Complete minigame)";

            case QuestData.questObj.talkToNPC:

            if (quest.npcsToTalkTo != null && quest.npcsToTalkTo.Count > 0)
            {
                return $" ({progress}/{quest.npcsToTalkTo.Count} NPCs talked to)";
            }
            else if (quest.npcToTalkTo != null)
            {
                return $" (Talk to {quest.npcToTalkTo.npcName})";
            }
            else
            {
                return $" ({progress}/{quest.requiredAmount})";
            }

            default:
                return "";
        }
    }
}
