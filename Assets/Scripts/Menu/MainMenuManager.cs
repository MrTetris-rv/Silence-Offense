using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public void OnCreateAServer()
    {
        SceneManager.LoadScene("Game");
    }
    public void OnJoinServer()
    {

    }
    public void OnSettings()
    {

    }

    public void OnExit()
    {
        Application.Quit();
    }
}
