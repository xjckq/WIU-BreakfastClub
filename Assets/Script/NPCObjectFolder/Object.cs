using UnityEngine;

public class Object : MonoBehaviour
{
    public ObjectData objectData;
    public Dialogue dialogueUI;

    public bool isPlayerInRange = false;
    public GameObject emote;

    public bool AutoTrigger;


    void Update()
    {
        if (isPlayerInRange && Input.GetKey(KeyCode.F) && !AutoTrigger)
        {
            dialogueUI.StartDialogue(objectData.objectDialogueData.objectLines);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
            emote.SetActive(true);

            if (AutoTrigger)
            {
                dialogueUI.StartDialogue(objectData.objectDialogueData.objectLines);
                emote.SetActive(false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            emote.SetActive(false);
        }
    }
}
