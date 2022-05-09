using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float m_bulletLiftime;

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
            Destroy(gameObject);
        }

        if(collision.CompareTag("World"))
        {
            Destroy(gameObject);
        }
    }
}
