using System.Collections;
using UnityEngine;
using Unity.Cinemachine;
using System;

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
    public GameObject emote;

    [SerializeField] QuestData questData;

    public CinemachineCamera zoomOutCamera;
    public float zoomDuration = 5;

    [SerializeField] NPCPatrol patrol;

    void Start()
    {
        currentState = NPCDialogueManager.Instance.GetDialogueState(npcData.npcName);
        isFinished = NPCDialogueManager.Instance.GetIsFinished(npcData.npcName);
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKey(KeyCode.E))
        {
            TriggerDialogue();
        }

        //if (questData != null && QuestManager.Instance.IsQuestCompleted(questData))
        //{
        //    currentState = DialogueState.QuestCompleted;
        //    QuestManager.Instance.CompleteQuest(questData);
        //}

        if (questData != null && QuestManager.Instance.IsQuestReadyToTurnIn(questData))
        {
            if (!isFinished)
            {
                currentState = DialogueState.QuestCompleted;
            }
            else if (isFinished && currentState == DialogueState.QuestCompleted)
            {
                currentState = DialogueState.Default;
            }
        }

    }

    public void TriggerDialogue()
    {
        if (patrol != null)
            patrol.isInDialogue = true; 

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
        if (patrol != null)
            patrol.isInDialogue = false;

        dialogueUI.OnDialogueFinished.RemoveListener(OnDialogueEnd);

        QuestManager.Instance.TalkedToNPC(this);

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
        else if (currentState == DialogueState.QuestCompleted)
        {
            if (questData != null)
            {

                if (zoomOutCamera != null)
                {
                    zoomOutCamera.gameObject.SetActive(true);
                    StartCoroutine(ZoomOutCam());
                }
                else
                {
                    QuestManager.Instance.CompleteQuest(questData);
                    currentState = DialogueState.Default;
                }

            }
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

    private IEnumerator ZoomOutCam()
    {
        yield return new WaitForSeconds(zoomDuration / 2);
        QuestManager.Instance.CompleteQuest(questData);
        currentState = DialogueState.Default;
        yield return new WaitForSeconds(zoomDuration / 2);
        currentState = DialogueState.Default;
        zoomOutCamera.gameObject.SetActive(false);
    }
}
