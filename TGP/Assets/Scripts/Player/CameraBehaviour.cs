using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] private DungeonGenerator m_Generator;
    [SerializeField] private GameObject m_Player;

    private Camera m_MainCamera;

    void Start()
    {
        m_MainCamera = GetComponent<Camera>();
    }

    void Update()
    {
        //find the room the player is in and move the camera's transform to match the room
        Cell currentRoom = m_Generator.FindRoom(m_Player.transform.position);
        m_MainCamera.transform.position = new Vector3(currentRoom.RoomObject.transform.position.x, currentRoom.RoomObject.transform.position.y, -1);
    }
}
