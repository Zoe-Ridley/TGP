using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_timerText;

    float m_currentTime;
    static float m_staticTime;

    void Start()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Main")) // if the scene is main then set  time back to 0
        {
            m_staticTime = m_currentTime;
        }
        else //else the scene is not main, continue it
        {
            m_currentTime = m_staticTime;
        }
        
    }

    void Update()
    {
        m_currentTime += 1 * Time.deltaTime;
        m_staticTime = m_currentTime;
        DisplayTime(m_currentTime);

    }

    void DisplayTime(float timeToDisplay)
    {
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }

        // calculate the minutes & seconds.
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        m_timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
