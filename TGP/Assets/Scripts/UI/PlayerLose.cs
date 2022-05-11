using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerLose : MonoBehaviour
{
    [SerializeField] private int m_playerHitpoints;
    [SerializeField] private Text m_textHitCounter;
    [SerializeField] private float m_invulnerableTime;

    [SerializeField] private Image m_playerHealthBar;

    bool m_invulnerable;
    float m_invTimer;

    void Start()
    {
        m_playerHealthBar = GetComponent<Image>();
    }

    void Update()
    {
        m_playerHealthBar.fillAmount = m_playerHitpoints;
        m_textHitCounter.text = "HP: " + m_playerHitpoints;

        if (m_playerHitpoints <= 0)
        {
            SceneManager.LoadScene("Lose");
            FindObjectOfType<AudioManager>().playAudio("Loss");
            FindObjectOfType<AudioManager>().StopAudio("MainMusic");
        }

        if (m_invTimer <= m_invulnerableTime)
        {
            m_invTimer += Time.deltaTime;
        }
        else
        {
            m_invTimer = 0.0f;
            m_invulnerable = false;
        }
    }

    // for trigger collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string enemyType = collision.gameObject.tag;

        if (!m_invulnerable)
        {
            switch (enemyType)
            {
                case "Bullet":
                    m_playerHitpoints--;
                    FindObjectOfType<AudioManager>().playAudio("PlayerHit");
                    m_invulnerable = true;
                    break;
            }
        }
    }

    // For kinematic or dynamic collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        string enemyType = collision.gameObject.tag;

        if (!m_invulnerable)
        {
            switch (enemyType)
            {
                case "MeleeEnemy":
                    m_playerHitpoints--;
                    FindObjectOfType<AudioManager>().playAudio("PlayerHit");
                    m_invulnerable = true;
                    break;
            }
        }
    }
}