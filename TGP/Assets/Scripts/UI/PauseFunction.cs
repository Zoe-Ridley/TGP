using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseFunction : MonoBehaviour
{
    [SerializeField] private GameObject m_pausePanel;
    private bool m_isPaused;

    private void Start()
    {
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P) && m_isPaused == false)
        {
            m_isPaused = true;
            m_pausePanel.SetActive(true);
            Time.timeScale = 0f;
        }
        else if (Input.GetKeyDown(KeyCode.P) && m_isPaused == true)
        {
            m_isPaused = false;
            m_pausePanel.SetActive(false);
            Time.timeScale = 1f;

        }
    }

    public void ResumeButton()
    {
        m_isPaused = false;
        m_pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
