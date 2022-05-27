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
    public float m_startTime;

    void Start()
    {
        m_currentTime = m_startTime;
    }

    void Update()
    {
        m_currentTime += 1 * Time.deltaTime;
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
