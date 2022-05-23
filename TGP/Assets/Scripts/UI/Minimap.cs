using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public float angle;

    public Transform minimapOverlay;
    private Transform player;
    private GameObject[] enemies;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    void Update()
    {
        transform.position = player.position + Vector3.up * 5f;

        HandleEnemyVisible();
        RotateOverlay();
    }

    private void HandleEnemyVisible()
    {
        //Disables enemies sprite if the angle between the player's forward and vector to enemy position is greater than half the camera angle.
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].SetActive(Vector3.Angle(player.forward, enemies[i].transform.position - player.position) <= angle);
        }
    }

    private void RotateOverlay()
    {
        //Rotate the minimap overlay based on player's rotation.
       // minimapOverlay.localRotation = Quaternion.Euler(0, 0, -player.eulerAngles.y - angle);
    }
}
