using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToStartGameScene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Home")
            StartCoroutine(NextScene());
        if (SceneManager.GetActiveScene().name == "EndingCutscene")
            StartCoroutine(AfterEndingCutscene());
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.L))
        {
            if (SceneManager.GetActiveScene().name == "Home")
                SceneManager.LoadScene("DialogueScene");
            if (SceneManager.GetActiveScene().name == "EndingCutscene")
                SceneManager.LoadScene("MainMenu");
        }
    }

    private IEnumerator NextScene(){
        yield return new WaitForSeconds(27.0f); // original wait 35.5 [changed to 27.0 cuz i made the dialogue faster!!]
        SceneManager.LoadScene("DialogueScene");
    }

    private IEnumerator AfterEndingCutscene(){
        yield return new WaitForSeconds(65.0f);
        SceneManager.LoadScene("MainMenu");
    }
}
