using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField] private GameObject m_CirclePrefab;
    [SerializeField] private int m_Damage;
    private GameObject m_fillCircle;
    private GameObject m_player;
    Vector3 m_CircleSize;
    bool m_colliding;
    Material m_material;

    // Start is called before the first frame update
    void Start()
    {
        m_fillCircle = Instantiate(m_CirclePrefab, transform.position, Quaternion.identity);
        m_CirclePrefab.transform.localScale = Vector3.zero;
        m_CircleSize = new Vector3();
        m_material = GetComponent<SpriteRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(m_CircleSize.x);
        m_CircleSize.x += 5f * Time.deltaTime;
        m_CircleSize.y += 5f * Time.deltaTime;
        m_fillCircle.transform.localScale = m_CircleSize;

        m_material.SetVector("_CenterPosition", transform.position);
        m_material.SetFloat("_Radius", GetComponent<CircleCollider2D>().radius);

        if (m_fillCircle.transform.localScale.x >= transform.localScale.x)
        {
            if (m_colliding)
            {
                if (m_player)
                {
                    m_player.GetComponent<PlayerLose>().PlayerHitPoints -= m_Damage;
                }
            }

            Destroy(m_fillCircle);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            m_colliding = true;
            m_player = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            m_colliding = false;
            m_player = null;
        }
    }
}
