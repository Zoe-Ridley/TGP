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
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(m_player.transform.position.x - 1f, m_player.transform.position.y - 1f),
                            Vector2.left, 1000.0f);
            Debug.Log(hit.point);
            Debug.Log("click! :D");
            if (hit.collider != null)
            {
                GameObject tempObject = hit.transform.parent.transform.parent.gameObject;
                Debug.Log(tempObject.name);
                m_dungeonGenerator.OpenRoom(tempObject);
            }
        }
    }
}
