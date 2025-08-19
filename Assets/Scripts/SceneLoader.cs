using System.Collections;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] Animator transitionAnim;
    [SerializeField] private float transitionTime = 1f;

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
