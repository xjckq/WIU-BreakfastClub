using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] Animator transitionAnim;
    [SerializeField] private float transitionTime = 1f;
    [SerializeField] private Transform playerTransform;

    [SerializeField] private PlayerData playerData;

    public void LoadSceneAt(string sceneName, Vector3 spawnPoint)
    {
        if (spawnPoint != null)
        {
            playerData.spawnPosition = spawnPoint;
            playerData.hasCustomSpawn = true;
        }

        LoadScene(sceneName);
    }

    public void LoadAtDoor()
    {
        LoadSceneAt("DialogueScene", new Vector3(0.5f, 15, 0));
    }
    public void LoadOutsideHome()
    {
        LoadSceneAt("Outside", new Vector3(0, 1, 0));
    }
    public void LoadOutsideCTC()
    {
        LoadSceneAt("Outside", new Vector3(-6, -8, 0));
    }

    public void LoadScene(string sceneName)
    {
        //SceneManager.LoadScene(sceneName);
        if (transitionAnim != null)
            StartCoroutine(PlayTransition(sceneName));
        else
            LoadingScreenManager.loadingInstance.SwitchToScene(sceneName);
    }

    IEnumerator PlayTransition(string sceneName)
    {
        // trigger fade in anim
        transitionAnim.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);

        // trigger fade out anim
        transitionAnim.ResetTrigger("Start");
        transitionAnim.SetTrigger("End");
        yield return new WaitForSeconds(transitionTime);

        // load scene
        LoadingScreenManager.loadingInstance.SwitchToScene(sceneName);
    }

    public void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
    }
}