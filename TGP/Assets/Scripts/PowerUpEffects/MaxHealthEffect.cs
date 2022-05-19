using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHealthEffect : MonoBehaviour
{
    [SerializeField]
    private Item MaxHealth;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
            Debug.Log("Hit");
            FindObjectOfType<AudioManager>().playAudio("Power Up");
            float tempMaxHP = collision.gameObject.GetComponent<PlayerLose>().m_playerMaxHP;
            tempMaxHP *= MaxHealth.m_ChangeNum;
            collision.gameObject.GetComponent<PlayerLose>().m_playerMaxHP = Mathf.FloorToInt(tempMaxHP);
        }
    }
}
