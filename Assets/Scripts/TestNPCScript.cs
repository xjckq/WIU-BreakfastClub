using UnityEngine;

public class TestNPCScript : MonoBehaviour
{
    [SerializeField] QuestData questData;
    public bool playerIsNearby;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (playerIsNearby && Input.GetKeyDown(KeyCode.F))
        {
            QuestManager.Instance.StartQuest(questData);

            if (QuestManager.Instance.killEnemyCount == questData.requiredAmount)
                QuestManager.Instance.CompleteQuest(questData);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerIsNearby = true;
            Debug.Log("PLAYER");
        }

    }

    void OnTriggerExit2D(Collider2D collision)
    {
        playerIsNearby = false;
    }
}
