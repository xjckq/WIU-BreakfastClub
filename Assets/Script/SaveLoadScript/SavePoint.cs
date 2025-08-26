using UnityEngine;

public class SavePoint : MonoBehaviour
{
    public GameObject savePanel;
    public bool isPanelOpen = false;
    public bool isPlayerInRange = false;
    public GameObject emote; // the interactiev bubble

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isPlayerInRange)
        {
            if (!isPanelOpen)
            {
                savePanel.SetActive(true);
                isPanelOpen = true;
            }
            else
            {
                savePanel.SetActive(false);
                isPanelOpen = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
            emote.SetActive(true);
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

    public void OnSaveButton()
    {
        SaveManager.instance.SaveGame();
    }

    public void OnLoadButton()
    {
        SaveManager.instance.LoadGame();
    }

    public void OnDeleteButton()
    {
        SaveManager.instance.DeleteGameSave();
    }
}
