using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingScreenManager : MonoBehaviour
{
    public static LoadingScreenManager loadingInstance;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider progressBar;

    private void Awake()
    {
        // keep across scenes
        if (loadingInstance != null && loadingInstance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            loadingInstance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void SwitchToScene(string sceneName)
    {
        loadingScreen.SetActive(true);
        progressBar.value = 0;
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9f)
        {
            float targetProgress = asyncLoad.progress / 0.9f;
            progressBar.value = Mathf.MoveTowards(progressBar.value, targetProgress, Time.deltaTime);
            yield return null;
        }

        while (progressBar.value < 1f)
        {
            progressBar.value = Mathf.MoveTowards(progressBar.value, 1f, Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        asyncLoad.allowSceneActivation = true;

        // wait for scene to load 
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // hide loading screen
        loadingScreen.SetActive(false);
    }
}
