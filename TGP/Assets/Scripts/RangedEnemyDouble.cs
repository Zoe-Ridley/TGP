using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyDouble : MonoBehaviour
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

        m_tempFirerate = m_Firerate;
    }

    void Update()
    {
        if (m_player.gameObject.transform.position.y < gameObject.transform.position.y)
        {
            m_RB.AddForce(Vector2.down * m_enemySpeed, ForceMode2D.Force);
        }
        if (m_player.gameObject.transform.position.y > gameObject.transform.position.y)
        {
            m_RB.AddForce(Vector2.up * m_enemySpeed, ForceMode2D.Force);
        }

        m_tempFirerate -= Time.deltaTime;
        if ((m_player.gameObject.transform.position.y >= gameObject.transform.position.y - 1) && (m_tempFirerate <= 0))
        {
            GameObject tempRef = Instantiate<GameObject>(m_bulletPrefab, gameObject.transform.position, gameObject.transform.rotation);
            tempRef.GetComponent<Rigidbody2D>().AddForce(tempRef.transform.right * m_FiringForce, ForceMode2D.Impulse);
            GameObject tempRef1 = Instantiate<GameObject>(m_bulletPrefab, gameObject.transform.position, gameObject.transform.rotation);
            tempRef1.GetComponent<Rigidbody2D>().AddForce(tempRef1.transform.right * -m_FiringForce, ForceMode2D.Impulse);
            m_tempFirerate = m_Firerate;
        }
        if ((m_player.gameObject.transform.position.y <= gameObject.transform.position.y + 1) && (m_tempFirerate <= 0))
        {
            GameObject tempRef = Instantiate<GameObject>(m_bulletPrefab, gameObject.transform.position, gameObject.transform.rotation);
            tempRef.GetComponent<Rigidbody2D>().AddForce(tempRef.transform.right * m_FiringForce, ForceMode2D.Impulse);
            GameObject tempRef1 = Instantiate<GameObject>(m_bulletPrefab, gameObject.transform.position, gameObject.transform.rotation);
            tempRef1.GetComponent<Rigidbody2D>().AddForce(tempRef1.transform.right * -m_FiringForce, ForceMode2D.Impulse);
            m_tempFirerate = m_Firerate;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerBullet"))
        {
            Destroy(gameObject);
        }
    }
}