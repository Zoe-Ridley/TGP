using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeDamage : MonoBehaviour
{

    public GameObject m_player = null;
    public GameObject m_enemy = null;

    private void Awake()
    {
        m_player = GetComponent<GameObject>();
        m_enemy = GetComponent<GameObject>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            Debug.Log("hit");
            //Destroy(other.gameObject);
            UIsystem.instance.MeleeDamage(m_enemy);
        }
    }
}
