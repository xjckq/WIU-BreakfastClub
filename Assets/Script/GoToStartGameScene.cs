using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToStartGameScene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(NextScene());
    }

    
    private IEnumerator NextScene(){
        yield return new WaitForSeconds(35.5f);
        SceneManager.LoadScene("DialogueScene");
    }
}
