using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPotionEffect : MonoBehaviour
{
    [SerializeField]
    private Item HealingPotion;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Destroy(gameObject);
            Debug.Log("Hit");
            int m_TempHP = collision.gameObject.GetComponent<PlayerLose>().m_playerHitpoints + Mathf.FloorToInt(HealingPotion.m_ChangeNum);
            int m_TempMaxHp = collision.gameObject.GetComponent<PlayerLose>().m_playerMaxHP;
            if (m_TempHP > m_TempMaxHp)
            {
                collision.gameObject.GetComponent<PlayerLose>().m_playerHitpoints = m_TempMaxHp;
            }
        }
    }
}
