using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    [SerializeField] private float m_bulletLifetime;

    // Update is called once per frame
    void Update()
    {
        m_bulletLifetime -= Time.deltaTime;
        if (m_bulletLifetime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "Player") || (collision.gameObject.tag == "World"))
        {
            Destroy(gameObject);
        }
    }
}
