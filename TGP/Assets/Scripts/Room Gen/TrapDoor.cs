using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrapDoor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            SceneManager.LoadScene("BossLevel");
            FindObjectOfType<AudioManager>().playAudio("Boss Theme");
            FindObjectOfType<AudioManager>().StopAudio("MainMusic");
            FindObjectOfType<AudioManager>().StopAudio("Title");
        }
    }
}
