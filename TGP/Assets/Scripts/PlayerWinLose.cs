using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerWinLose : MonoBehaviour
{
    [SerializeField] private int m_playerHitpoints;

    void Update()
    {
        if (m_playerHitpoints <= 0)
        {
            SceneManager.LoadScene("Lose");
            FindObjectOfType<AudioManager>().playAudio("Loss");
            FindObjectOfType<AudioManager>().StopAudio("MainMusic");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            FindObjectOfType<AudioManager>().playAudio("PlayerHit");
            m_playerHitpoints -= 1;
        }
    }
}
