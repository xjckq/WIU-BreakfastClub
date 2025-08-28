using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class NPCDialogueManager : MonoBehaviour
{
    public static NPCDialogueManager Instance;

    [SerializeField]
    private List<DialogueStatePersist> npcStates = new List<DialogueStatePersist>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveDialogueState(string npcID, DialogueState state, bool isFinished)
    {
        DialogueStatePersist theState = null;
        foreach (var n in npcStates)
        {
            if (n.npcID == npcID)
            {
                theState = n;
                break;
            }
        }

        if (theState != null)
        {
            theState.state = state;
            theState.isFinished = isFinished;
        }
        else
        {
            DialogueStatePersist newEntry = new DialogueStatePersist();
            newEntry.npcID = npcID;
            newEntry.state = state;
            newEntry.isFinished = isFinished;
            npcStates.Add(newEntry);
        }
    }

    public DialogueState GetDialogueState(string npcID)
    {
        DialogueStatePersist theState = null;
        foreach (var n in npcStates)
        {
            if (n.npcID == npcID)
            {
                theState = n;
                break;
            }
        }

        if (theState != null)
        {
            return theState.state;
        }
        else
        {
            return DialogueState.FirstTalk;
        }
    }

    public bool GetIsFinished(string npcID)
    {
        foreach (var n in npcStates)
        {
            if (n.npcID == npcID)
            {
                return n.isFinished;
            }
        }
        return false;
    }

    public void SetDialogueState(string npcID, DialogueState state, bool isFinished)
    {
        SaveDialogueState(npcID, state, isFinished);
    }

    public List<NPCSaveData> GetDialogueSaveData()
    {
        List<NPCSaveData> result = new List<NPCSaveData>();
        foreach (var state in npcStates)
        {
            result.Add(new NPCSaveData
            {
                npcID = state.npcID,
                state = state.state,
                isFinished = state.isFinished
            });
        }
        return result;
    }
}
