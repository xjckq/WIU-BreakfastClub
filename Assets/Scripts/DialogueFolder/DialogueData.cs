using UnityEngine;

[CreateAssetMenu(fileName = "DialogueData", menuName = "Scriptable Objects/DialogueData")]
public class DialogueData : ScriptableObject
{
    public DialoguePack[] dialogueLines;
    public DialoguePack[] FirstTalkLines;
    public DialoguePack[] QuestOngoingLines;
    public DialoguePack[] QuestCompletedLines;
}
