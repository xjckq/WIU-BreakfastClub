using UnityEngine;
using UnityEngine.Events;

public class SavePoint : MonoBehaviour
{
    public GameObject savePanel;
    public bool isPanelOpen = false;
    public bool isPlayerInRange = false;
    public GameObject emote; // the interactiev bubble

    private PlayerController player;

    void Start()
    {
        GameObject thePlayer = GameObject.FindGameObjectWithTag("Player");
        if (thePlayer != null)
            player = thePlayer.GetComponent<PlayerController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isPlayerInRange)
        {
            if (!isPanelOpen)
            {
                savePanel.SetActive(true);
                isPanelOpen = true;
                if (player != null)
                    player.canMove = false;
            }
            else
            {
                savePanel.SetActive(false);
                isPanelOpen = false;
                if (player != null)
                    player.canMove = true;
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
}
