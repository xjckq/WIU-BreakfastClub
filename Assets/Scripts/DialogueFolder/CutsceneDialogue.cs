using System.Collections;
using UnityEngine;

public class CutsceneDialogue : MonoBehaviour
{
    public DialogueC dialogueUI;
    public ObjectData objectData;

    public bool playOnSceneStart = true;
    private bool hasPlayed = false;

    void OnEnable()
    {
        if (playOnSceneStart && !hasPlayed)
        {
            hasPlayed = true;
            dialogueUI.StartDialogue(objectData.objectDialogueData.objectLines, true);
        }
        else if (hasPlayed)
        {
            dialogueUI.AutoNextLine();
        }
    }
}
