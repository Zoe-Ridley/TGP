using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public float angle;

    public Transform minimapOverlay;
    private Transform player;
    private GameObject[] enemies;

    [SerializeField] private DungeonGenerator dungeonGenerator;
    //[SerializeField] private GameObject minimapCamera;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        //minimapCamera.transform.position = new Vector3(dungeonGenerator.CentreOfDungeon.x, dungeonGenerator.CentreOfDungeon.y);
    }

    void Update()
    {
        HandleEnemyVisible();
    }

    private void HandleEnemyVisible()
    {
        //Disables enemies sprite if the angle between the player's forward and vector to enemy position is greater than half the camera angle.
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].SetActive(Vector3.Angle(player.forward, enemies[i].transform.position - player.position) <= angle);
        }
    }


}
