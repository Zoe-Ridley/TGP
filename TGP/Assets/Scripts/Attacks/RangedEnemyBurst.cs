using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyBurst : MonoBehaviour
{
    [SerializeField] private GameObject m_player;
    [SerializeField] private GameObject m_bulletPrefab;
    [SerializeField] private float m_enemySpeed;
    [SerializeField] private float m_FiringForce;
    [SerializeField] private float m_Firerate;


    private float m_tempFirerate;
    private Rigidbody2D m_RB;

    void Start()
    {
        m_RB = GetComponent<Rigidbody2D>();
        m_player = GameObject.Find("Player");
        m_tempFirerate = m_Firerate;
    }

    void Update()
    {
        //Follow player Y
        if (m_player.transform.position.y < gameObject.transform.position.y)
        {
            m_RB.AddForce(Vector2.down * m_enemySpeed, ForceMode2D.Force);
        }
        if (m_player.transform.position.y > gameObject.transform.position.y)
        {
            m_RB.AddForce(Vector2.up * m_enemySpeed, ForceMode2D.Force);
        }

        m_tempFirerate -= Time.deltaTime; //cooldown

        Vector3 shootDirection = m_player.transform.position - transform.position; //Rotation
        float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
        if (m_tempFirerate <= 0)
        {
            Debug.Log("shoot");
            GameObject tempRef = Instantiate(m_bulletPrefab, transform.position, Quaternion.AngleAxis(angle, Vector3.forward));
            tempRef.GetComponent<Rigidbody2D>().AddForce(shootDirection * m_FiringForce, ForceMode2D.Impulse);
            m_tempFirerate = m_Firerate;
        }


        //if ((m_player.transform.position.y >= gameObject.transform.position.y - 1) && (m_tempFirerate <= 0) && (m_player.transform.position.x >= gameObject.transform.position.x))
        //{
        //    //RIGHT AND UP

        //    GameObject tempRef = Instantiate(m_bulletPrefab, gameObject.transform.position, gameObject.transform.rotation);
        //    tempRef.GetComponent<Rigidbody2D>().AddForce(tempRef.transform.right * m_FiringForce, ForceMode2D.Impulse);
        //    m_tempFirerate = m_Firerate;
        //}

        //else if ((m_player.transform.position.y <= gameObject.transform.position.y + 1) && (m_tempFirerate <= 0) && (m_player.transform.position.x <= gameObject.transform.position.x))
        //{
        //    //LEFT AND DOWN

        //    GameObject tempRef1 = Instantiate(m_bulletPrefab, gameObject.transform.position, gameObject.transform.rotation);
        //    tempRef1.GetComponent<Rigidbody2D>().AddForce(tempRef1.transform.right * -m_FiringForce, ForceMode2D.Impulse);
        //    m_tempFirerate = m_Firerate;
        //}


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBullet"))
        {
            Destroy(gameObject);
            FindObjectOfType<AudioManager>().playAudio("EnemyDeath");
        }
    }
}