using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpEffect : MonoBehaviour
{
    public Item SpeedUp;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
            Debug.Log("Hit");
            FindObjectOfType<AudioManager>().playAudio("Power Up");
            collision.gameObject.GetComponent<PlayerMovement>().PlayerSpeed *= SpeedUp.m_ChangeNum;
        }
    }
}
