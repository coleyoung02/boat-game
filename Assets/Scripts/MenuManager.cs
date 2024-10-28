using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void Exit()
    {
        Application.Quit();
    }

    public void Again()
    {
        SceneManager.LoadScene("main");
    }
}
