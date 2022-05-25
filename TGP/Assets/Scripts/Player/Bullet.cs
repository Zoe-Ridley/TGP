using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float m_bulletLiftime;
    [SerializeField] private GameObject m_wallCollision;
    [SerializeField] private GameObject m_EnenyCollision;

    private void Update()
    {
        m_bulletLiftime -= Time.deltaTime;
        if(m_bulletLiftime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            Instantiate(m_EnenyCollision, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        if(collision.CompareTag("World"))
        {
            Instantiate(m_wallCollision, transform.position, this.transform.rotation);
            Destroy(gameObject);
        }

        if (collision.CompareTag("MeleeEnemy"))
        {
            Instantiate(m_EnenyCollision, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
