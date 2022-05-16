using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHealthEffect : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
            Debug.Log("Hit");
            //FindObjectOfType<UIsystem>().
        }
    }
}
