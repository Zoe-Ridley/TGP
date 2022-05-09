using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.UI;

public class RangedEnemyDouble : MonoBehaviour
{
    [SerializeField] private GameObject m_player;
    [SerializeField] private GameObject m_bulletPrefab;
    [SerializeField] private float m_enemySpeed;
    [SerializeField] private float m_FiringForce;
    [SerializeField] private float m_Firerate;
    [SerializeField] private int m_enemyHP;
    //[SerializeField] private Text m_enemyKillCounter;

    private int m_enemyCount = 6;
    static int m_enemyKillCount;
    

    private float m_tempFirerate;
    private Rigidbody2D m_RB;

    void Start()
    {
        m_RB = GetComponent<Rigidbody2D>();
        m_player = GameObject.Find("Player");
        m_tempFirerate = m_Firerate;
        m_enemyKillCount = 0;
        Debug.Log(m_enemyCount);
    }

    void Update()
    {
        if (m_player.transform.position.y < gameObject.transform.position.y)
        {
            m_RB.AddForce(Vector2.down * m_enemySpeed, ForceMode2D.Force);
        }
        if (m_player.transform.position.y > gameObject.transform.position.y)
        {
            m_RB.AddForce(Vector2.up * m_enemySpeed, ForceMode2D.Force);
        }

        m_tempFirerate -= Time.deltaTime;
        if (((m_player.transform.position.y >= gameObject.transform.position.y - 1) || m_player.transform.position.y <= gameObject.transform.position.y + 1) && (m_tempFirerate <= 0) )
        {
            GameObject tempRef = Instantiate<GameObject>(m_bulletPrefab, gameObject.transform.position, gameObject.transform.rotation);
            tempRef.GetComponent<Rigidbody2D>().AddForce(tempRef.transform.right * m_FiringForce, ForceMode2D.Impulse);
            GameObject tempRef1 = Instantiate<GameObject>(m_bulletPrefab, gameObject.transform.position, gameObject.transform.rotation);
            tempRef1.GetComponent<Rigidbody2D>().AddForce(tempRef1.transform.right * -m_FiringForce, ForceMode2D.Impulse);
            m_tempFirerate = m_Firerate;
        }
        if (m_enemyKillCount == m_enemyCount)
        {
            SceneManager.LoadScene("Win");
            FindObjectOfType<AudioManager>().playAudio("Win");
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBullet") && m_enemyHP != 2)
        {
            Debug.Log("enenmy hit" + m_enemyHP);
            FindObjectOfType<AudioManager>().playAudio("EnemyDeath");
            m_enemyHP += 1;
        }
            if (collision.CompareTag("PlayerBullet") && m_enemyHP == 2)
        {
            m_enemyKillCount += 1;
            m_enemyHP = 0;
            Debug.Log(m_enemyKillCount);
            Destroy(gameObject);
            FindObjectOfType<AudioManager>().playAudio("EnemyDeath");
            //m_enemyKillCounter.text = "Orange kills: " + m_enemyKillCount;
        }
        if (collision.CompareTag("PlayerBullet") && m_enemyKillCount == m_enemyCount)
        {
            Destroy(gameObject);
            //m_enemyKillCounter.text = "Orange kills: " + m_enemyKillCount;
            FindObjectOfType<AudioManager>().playAudio("EnemyDeath");
            FindObjectOfType<AudioManager>().playAudio("Win");
            FindObjectOfType<AudioManager>().StopAudio("MainMusic");
            SceneManager.LoadScene("Win");
        }
    }
}