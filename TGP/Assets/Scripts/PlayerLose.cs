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

    bool m_invulnerable;
    float m_invTimer;

    void Update()
    {
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string enemyType = collision.gameObject.tag;

        if (!m_invulnerable)
        {
            switch (enemyType)
            {
                case "Bullet":
                    m_playerHitpoints -= 1;
                    FindObjectOfType<AudioManager>().playAudio("PlayerHit");
                    m_invulnerable = true;
                    break;
                case "MeleeEnemy":
                    m_playerHitpoints -= 1;
                    FindObjectOfType<AudioManager>().playAudio("PlayerHit");
                    m_invulnerable = true;
                    break;
            }
        }
    }
}