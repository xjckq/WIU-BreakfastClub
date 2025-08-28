
using UnityEngine;
using TMPro;
public class CTCManager : MonoBehaviour
{
    public static CTCManager Instance;
    float timer;
    public int capturedNum;
    public int escapedNum;
    [SerializeField] TMP_Text capturedNumTxt;
    [SerializeField] TMP_Text escapedNumTxt;
    [SerializeField] TMP_Text acquiredNumTxt;
    [SerializeField] TMP_Text timerTxt;
    [SerializeField] GameObject resultsPanel;

    public PlayerData playerData;
    public ItemData eggItemData;

    public bool gameOver;
    bool eggsGiven;
    public int totalChickens = 4;
    float soundTimer = 3;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip chickenSound;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 1;
        resultsPanel.SetActive(false);
        timer = 30;
        gameOver = false;
        eggsGiven = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {
            timer -= Time.deltaTime;
            soundTimer -= Time.deltaTime;
            if (soundTimer <= 0)
            {
                audioSource.PlayOneShot(chickenSound);
                soundTimer = 3;
            }
            timerTxt.text = Mathf.Ceil(timer).ToString();
            if (timer <= 0 || (capturedNum + escapedNum >= totalChickens))
            {
                gameOver = true;
                displayResults();
            }

        }
    }


    private void displayResults()
    {
        if (eggsGiven)
            return;

        resultsPanel.SetActive(true);
        for (int i = 0; i < capturedNum; i++)
        {
            ItemInstance egg = new ItemInstance(eggItemData, null);
            playerData.inventoryItems.Add(egg);
        }

        UpdateUI();
        eggsGiven = true;
        QuestManager.Instance.MinigameCompleted(QuestData.MinigameType.Chicken);
    }

    public void UpdateUI()
    {
        capturedNumTxt.text = capturedNum.ToString();
        escapedNumTxt.text = escapedNum.ToString();
        acquiredNumTxt.text = capturedNum.ToString();
    }

    public void Captured()
    {
        capturedNum++;
        UpdateUI();
    }

    public void Escaped()
    {
        escapedNum++;
        UpdateUI();
    }
}
