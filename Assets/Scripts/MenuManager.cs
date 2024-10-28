using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private bool isPauseMenu;
    [SerializeField] private GameObject overlay;
    private bool isPaused = false;

    public bool GetPaused()
    {
        return isPaused;
    }

    private void Update()
    {
        if (isPauseMenu)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!isPaused)
                {
                    overlay.SetActive(true);
                    Time.timeScale = 0f;
                    isPaused = true;
                }
                else
                {
                    Resume();
                }
            }
        }
    }
    public void Exit()
    {
        Application.Quit();
    }

    public void Again()
    {
        SceneManager.LoadScene("main");
    }

    public void Resume()
    {
        overlay.SetActive(false);
        Time.timeScale = 1.0f;
        isPaused = false;
    }
}
