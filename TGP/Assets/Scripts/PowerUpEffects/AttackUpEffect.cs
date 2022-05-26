using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackUpEffect : MonoBehaviour
{
    public Item AttackUp;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
            Debug.Log("Hit");
            FindObjectOfType<AudioManager>().playAudio("Power Up");
            collision.gameObject.GetComponent<Shoot>().m_reloadTime *= AttackUp.m_ChangeNum;
        }
    }
}
