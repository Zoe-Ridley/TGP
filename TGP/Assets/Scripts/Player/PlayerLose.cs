using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerLose : MonoBehaviour
{
    public int m_playerMaxHP = 100;
    [SerializeField] public int m_playerHitpoints;
    [SerializeField] private TextMeshProUGUI m_textHitCounter;
    [SerializeField] private float m_invulnerableTime;

    [Header("Slider")]
    public Slider slider;
    public Image fill;

    bool m_invulnerable;
    float m_invTimer;

    private Animator m_animator;

    void Start()
    {
        slider = slider.GetComponent<Slider>();
        fill = slider.GetComponent<Image>();
        m_playerHitpoints = 100;
        slider.value = m_playerHitpoints;
    }

    void Update()
    {
        slider.value = m_playerHitpoints;
        m_textHitCounter.SetText(" " + m_playerHitpoints);

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
                    m_animator.SetTrigger("isAttacking");
                    FindObjectOfType<AudioManager>().playAudio("PlayerHit");
                    m_invulnerable = true;
                    break;
            }
        }
    }
}