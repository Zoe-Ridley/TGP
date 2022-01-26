using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class RoomUnlock : MonoBehaviour
{
    [SerializeField] private DungeonGenerator m_dungeonGenerator;
    [SerializeField] private GameObject m_player;


    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_dungeonGenerator.OpenRoom(new Vector2(m_player.transform.position.x, m_player.transform.position.y));
        }
    }
}
