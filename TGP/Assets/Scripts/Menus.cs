using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour
{
    public void BackButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitButton()
    {
        Debug.Log("Exit");
        Application.Quit();
    }

    public void SettingsButton()
    {
        SceneManager.LoadScene("Settings");
    }

    public void StartButton()
    {
        Debug.Log("Start"); //Remove me when load scene is added
        //INSERT LOAD GAME SCENE HERE
        SceneManager.LoadScene("James");
    }
}
