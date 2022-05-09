using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class ButtonManager : MonoBehaviour
{
    public void StartButton()
    {
        SceneManager.LoadScene("Main");
        FindObjectOfType<AudioManager>().playAudio("MainMusic");
        FindObjectOfType<AudioManager>().StopAudio("Title");
    }
    public void SettingsButton()
    {
        SceneManager.LoadScene("Settings");
        FindObjectOfType<AudioManager>().playAudio("ButtonPress");
    }
    public void ExitButton()
    {
        Debug.Log("Exit");
        FindObjectOfType<AudioManager>().playAudio("ButtonPress");
        Application.Quit();
    }
    public void BackButton()
    {
        SceneManager.LoadScene("MainMenu");
        FindObjectOfType<AudioManager>().playAudio("ButtonPress");
    }

    public void ResetButton()
    {
        SceneManager.LoadScene("MainMenu");
        FindObjectOfType<AudioManager>().playAudio("ButtonPress");
        FindObjectOfType<AudioManager>().playAudio("Title");
        FindObjectOfType<AudioManager>().StopAudio("Loss");
        FindObjectOfType<AudioManager>().StopAudio("Win");
    }
}
