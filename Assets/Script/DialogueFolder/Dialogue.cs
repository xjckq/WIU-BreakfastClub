using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct DialoguePack
{
    public string characterName;
    public string diaTxt;
}


public class Dialogue : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueTxt;
    [SerializeField] private TextMeshProUGUI characterNameTxt;
    [SerializeField] private GameObject panel;

    private DialoguePack[] dialoguelines;
    private int currLine = 0;
    private bool isTyping = false;

    private PlayerController player;

    public UnityEvent OnDialogueFinished;

    public void StartDialogue(DialoguePack[] dialogueLines)
    {
        dialoguelines = dialogueLines;
        currLine = 0;
        OpenDialogue();
        ShowCurrLine();
    }

    void Start()
    {
        currLine = 0;
        if (OnDialogueFinished == null)
            OnDialogueFinished = new UnityEvent();

        if (dialoguelines != null && dialoguelines.Length > 0)
        {
            ShowCurrLine();
        }

        GameObject thePlayer = GameObject.FindGameObjectWithTag("Player");
        if (thePlayer != null)
            player = thePlayer.GetComponent<PlayerController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                dialogueTxt.text = dialoguelines[currLine].diaTxt;
                isTyping = false;
            }
            else
            {
                GoToNextLine();
            }
        }
    }

    private void ShowCurrLine()
    {
        //dialogueTxt.text = dialoguelines[currLine].diaTxt;
        //characterNameTxt.text = dialoguelines[currLine].characterName;
        StopAllCoroutines();
        characterNameTxt.text = dialoguelines[currLine].characterName;
        StartCoroutine(TypeLine(dialoguelines[currLine].diaTxt));
    }

    private void GoToNextLine()
    {
        currLine++;
        if (currLine < dialoguelines.Length)
        {
            ShowCurrLine();
        }
        else
        {
            CloseDialogue();
        }
    }

    private IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueTxt.text = "";

        foreach (char c in line)
        {
            dialogueTxt.text += c;
            yield return new WaitForSeconds(0.03f);
        }

        isTyping = false;
    }

    public void OpenDialogue()
    {
        if (player != null)
            player.canMove = false;
        panel.SetActive(true);
    }

    public void CloseDialogue()
    {
        if (player != null)
            player.canMove = true;
        panel.SetActive(false);
        currLine = 0;

        OnDialogueFinished.Invoke();
    }
}