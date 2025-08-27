using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueC : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueTxt;
    [SerializeField] private TextMeshProUGUI characterNameTxt;
    [SerializeField] private GameObject panel;

    private DialoguePack[] dialoguelines;
    private int currLine = 0;
    private bool isTyping = false;
    private bool isPlayingCutscene = false;

    public void StartDialogue(DialoguePack[] dialogueLines, bool autoPlay = false)
    {
        dialoguelines = dialogueLines;
        isPlayingCutscene = autoPlay;
        OpenDialogue();
        ShowCurrLine();
    }

    void Start()
    {
        currLine = 0;

        if (dialoguelines != null && dialoguelines.Length > 0)
        {
            ShowCurrLine();
        }
    }

    void Update()
    {
        if (dialoguelines == null || dialoguelines.Length == 0 || isPlayingCutscene) return;

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
            yield return new WaitForSeconds(0.01f);
        }

        isTyping = false;

        if (isPlayingCutscene)
        {
            yield return new WaitForSeconds(2.0f);
            GoToNextLine();
        }
    }

    public void OpenDialogue()
    {
        panel.SetActive(true);
    }

    public void CloseDialogue()
    {
        panel.SetActive(false);
    }

    public void AutoNextLine()
    {
        if (dialoguelines == null || currLine >= dialoguelines.Length)
            return;

        currLine++;
        if (currLine < dialoguelines.Length)
        {
            ShowCurrLine();
        }
    }
}