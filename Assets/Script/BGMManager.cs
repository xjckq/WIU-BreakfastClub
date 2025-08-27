using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    public static BGMManager instance;

    public AudioSource audioSource;
    public AudioClip menuBGM;
    public AudioClip homeBGM;
    public AudioClip outsideBGM;

    private string currentScene;

    void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;
        UpdateMusicForScene(currentScene);
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            if (audioSource == null)
                audioSource = GetComponent<AudioSource>();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void OnDestroy()
    {
        if (instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene.name;
        UpdateMusicForScene(currentScene);
    }

    void UpdateMusicForScene(string sceneName)
    {
        Debug.Log("Scene loaded: " + sceneName);
        AudioClip changeBGM;

        switch (sceneName)
        {
            case "MainMenu":
                changeBGM = menuBGM;
                break;
            case "Home":
            case "DialogueScene":
                changeBGM = homeBGM;
                break;
            case "Outside":
                changeBGM = outsideBGM;
                break;
            default:
                changeBGM = null;
                break;
        }

        if (audioSource.clip != changeBGM)
        {
            StartCoroutine(FadeTrack(changeBGM));
        }
    }

    private IEnumerator FadeTrack(AudioClip newClip)
    {
        float startVolume = audioSource.volume;

        for (float i = 0; i < 0.4f; i += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, i);
            yield return null;
        }

        audioSource.Stop();
        audioSource.clip = newClip;

        if (newClip != null)
        {
            audioSource.Play();
        }

        for (float i = 0; i < 0.4f; i += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0f, startVolume, i);
            yield return null;
        }

        audioSource.volume = startVolume;
    }
}
