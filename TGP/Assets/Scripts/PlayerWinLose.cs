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
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            m_playerHitpoints -= 1;
        }
    }
}
