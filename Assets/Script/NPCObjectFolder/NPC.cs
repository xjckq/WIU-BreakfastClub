using System;
using UnityEngine;

public enum DialogueState
{
    FirstTalk,
    QuestOngoing,
    QuestCompleted,
    Default
}

[Serializable]
public class DialogueStatePersist
{
    public string npcID;
    public DialogueState state;
    public bool isFinished;
}

public class NPC : MonoBehaviour
{
    public NPCData npcData;
    public Dialogue dialogueUI;
    public DialogueState currentState = DialogueState.FirstTalk;
    public static NPC activeNPC;

    public bool isPlayerInRange = false;
    public bool isFinished = false;
    public GameObject emote; // the interactiev bubble

    [SerializeField] QuestData questData;

    void Start()
    {
        currentState = NPCDialogueManager.Instance.GetDialogueState(npcData.npcName);
        isFinished = NPCDialogueManager.Instance.GetIsFinished(npcData.npcName);
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKey(KeyCode.F))
        {
            TriggerDialogue();
        }

        if (questData != null && QuestManager.Instance.IsQuestCompleted(questData))
        {
            if (!isFinished)
            {
                currentState = DialogueState.QuestCompleted;
                QuestManager.Instance.CompleteQuest(questData);
            }
            else if (isFinished && currentState == DialogueState.QuestCompleted)
            {
                currentState = DialogueState.Default;
            }
            
        }
    }

    public void TriggerDialogue()
    {
        DialoguePack[] selectedDialogue;

        switch (currentState)
        {
            case DialogueState.FirstTalk:
                selectedDialogue = npcData.dialogueData.FirstTalkLines;
                break;
            case DialogueState.QuestOngoing:
                selectedDialogue = npcData.dialogueData.QuestOngoingLines;
                break;
            case DialogueState.QuestCompleted:
                selectedDialogue = npcData.dialogueData.QuestCompletedLines;
                break;
            default:
                selectedDialogue = npcData.dialogueData.dialogueLines;
                break;
        }

        dialogueUI.StartDialogue(selectedDialogue);
        dialogueUI.OnDialogueFinished.AddListener(OnDialogueEnd);
    }

    private void OnDialogueEnd()
    {
        dialogueUI.OnDialogueFinished.RemoveListener(OnDialogueEnd);

        if (currentState == DialogueState.FirstTalk)
        {
            if (questData != null)
            {
                currentState = DialogueState.QuestOngoing;

                QuestManager.Instance.StartQuest(questData);
            }
            else
            {
                currentState = DialogueState.Default;
            }
        }

        if (currentState == DialogueState.QuestCompleted)
        {
            isFinished = true;
        }
        
        NPCDialogueManager.Instance.SaveDialogueState(npcData.npcName, currentState, isFinished);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
            emote.SetActive(true);
            activeNPC = this;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            emote.SetActive(false);
            if (activeNPC == this)
                activeNPC = null;
        }
    }
}
