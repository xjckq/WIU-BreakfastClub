using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class CTCManager : MonoBehaviour
{

    float timer;
    public int capturedNum;
    public int escapedNum;
    [SerializeField] TMP_Text capturedNumTxt;
    [SerializeField] TMP_Text escapedNumTxt;
    [SerializeField] TMP_Text acquiredNumTxt;
    [SerializeField] TMP_Text timerTxt;
    [SerializeField] GameObject resultsPanel;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 1;
        resultsPanel.SetActive(false);
        timer = 30;
    }
    
    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        timerTxt.text = Mathf.Ceil(timer).ToString();
        if (timer <= 0)
        {
            Time.timeScale = 0;
            resultsPanel.SetActive(true);
            UpdateUI();
        }
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
