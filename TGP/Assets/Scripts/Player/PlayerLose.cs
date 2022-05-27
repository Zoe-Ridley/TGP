using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerLose : MonoBehaviour
{
    public int m_playerMaxHP;
    [SerializeField] public int m_playerHitpoints;
    [SerializeField] private TextMeshProUGUI m_textHitCounter;
    [SerializeField] private float m_invulnerableTime;
    static int m_playerMaxHPStatic;
    static int m_playerHitPointsStatic;

    [Header("Slider")]
    public Slider slider;
    public Image fill;

    bool m_invulnerable;
    float m_invTimer;

    private Animator m_animator;

    void Start()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Main")) // if the scene is main then set max health back to default
        {
            m_playerMaxHPStatic = m_playerMaxHP;
            m_playerHitPointsStatic = m_playerHitpoints;
        }
        else // if not main scene then set it to post powerups
        {
            m_playerMaxHP = m_playerMaxHPStatic;
            m_playerHitpoints = m_playerHitPointsStatic;
        }
        slider = slider.GetComponent<Slider>();
        fill = slider.GetComponent<Image>();
        slider.value = m_playerHitpoints;
        m_animator = GetComponent<Animator>();
    }

    void Update()
    {
        m_playerMaxHPStatic = m_playerMaxHP;
        m_playerHitPointsStatic = m_playerHitpoints;
        slider.value = m_playerHitpoints;
        slider.maxValue = m_playerMaxHP;
        m_textHitCounter.SetText(" " + m_playerHitpoints + "/" + m_playerMaxHP);


        if (m_playerHitpoints <= 0)
        {
            SceneManager.LoadScene("Lose");
            FindObjectOfType<AudioManager>().playAudio("Loss");
            FindObjectOfType<AudioManager>().StopAudio("MainMusic");
            FindObjectOfType<AudioManager>().StopAudio("Boss Theme");
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
                    m_playerHitpoints -= 10;
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
                    m_playerHitpoints -= 10;
                    m_animator.SetTrigger("isAttacking");
                    FindObjectOfType<AudioManager>().playAudio("PlayerHit");
                    m_invulnerable = true;
                    break;
            }
        }
    }
}