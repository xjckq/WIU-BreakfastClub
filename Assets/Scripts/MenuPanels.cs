using UnityEngine;

public class MenuPanels : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    private bool pauseOnOpen = true;

    private bool isOpen;

    public void TogglePanel()
    {
        isOpen = !isOpen;
        panel.SetActive(isOpen);

        if (pauseOnOpen)
            Time.timeScale = isOpen ? 0f : 1f;
    }

    public void OpenPanel()
    {
        isOpen = true;
        panel.SetActive(true);
        if (pauseOnOpen) Time.timeScale = 0f;
    }

    public void ClosePanel()
    {
        isOpen = false;
        panel.SetActive(false);
        if (pauseOnOpen) Time.timeScale = 1f;
    }
}