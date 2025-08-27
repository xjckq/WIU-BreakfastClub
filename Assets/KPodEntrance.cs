using UnityEngine;

public class KPodEntrance : MonoBehaviour
{
    public QuestData questData;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (QuestManager.Instance.IsQuestActive(questData))
        {
            Destroy(gameObject);
        }
    }
}
