using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance = null;
    [SerializeField] private GameObject pauseMenuPanel;

    private bool isPaused = false;

    void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }
    }

    public void Pause(bool isPaused)
    {
        if (isPaused)
        {
            pauseMenuPanel.SetActive(true);
            Time.timeScale = 0f;
            this.isPaused = isPaused;
        }
    }

    public void OnResume()
    {
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void OnQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    public bool GameIsPaused()
    {
        return isPaused;
    }
}
