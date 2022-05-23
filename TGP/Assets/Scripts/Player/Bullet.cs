using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float m_bulletLiftime;
    [SerializeField] private ParticleSystem m_wallCollision;
    [SerializeField] private AnimationCurve speedOverLife;

    private void Update()
    {
        m_bulletLiftime -= Time.deltaTime;
        if(m_bulletLiftime <= 0)
        {
            Destroy(gameObject);
        }


        float speed = speedOverLife.Evaluate(m_bulletLiftime);
        gameObject.GetComponent<Rigidbody2D>().AddForce(transform.right * speed, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }

        if(collision.CompareTag("World"))
        {
            m_wallCollision.Play();
            Destroy(gameObject);
        }
    }
}
