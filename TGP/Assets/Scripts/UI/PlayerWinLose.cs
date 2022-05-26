using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerWinLose : MonoBehaviour
{
    [SerializeField] private int m_playerHitpoints;
    //[SerializeField] private Text m_textHitCounter;
    [SerializeField] private Material m_dissolve;
    private float m_fadeValue;

    private void Start()
    {
        m_fadeValue = 1.0f;
    }

    void Update()
    {
        //m_textHitCounter.text = "HP: " + m_playerHitpoints;
        if (m_playerHitpoints <= 0)
        {
            // while the fade animation is still playing
            if (m_fadeValue > 0.0f)
            {
                GetComponent<SpriteRenderer>().material = m_dissolve;
                GetComponent<SpriteRenderer>().material.SetFloat("_Fade", m_fadeValue);
                m_fadeValue -= Time.deltaTime / 2;
            }
            // switch scenes once the fade animation is complete
            else
            {
                SceneManager.LoadScene("Lose");
                FindObjectOfType<AudioManager>().playAudio("Loss");
                FindObjectOfType<AudioManager>().StopAudio("MainMusic");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            m_playerHitpoints -= 1;
            FindObjectOfType<AudioManager>().playAudio("PlayerHit");
        }
    }

    /// <summary>
    /// function to retrieve players current health
    /// </summary>
    public int GetCurrentHealth()
    {
        return m_playerHitpoints;
    }
}
