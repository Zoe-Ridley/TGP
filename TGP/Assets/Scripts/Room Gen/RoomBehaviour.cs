using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RoomBehaviour : MonoBehaviour
{
    public GameObject[] m_walls; //0 Up, 1 Down, 2 Right, 3 Left
    public GameObject[] m_doors;
    [SerializeField] private GameObject m_LightManager;

    public Vector2 RoomSize;

    public void Start()
    {
        m_LightManager.SetActive(false);
    }

    //true = door closed
    //false = door open
    public void UpdateRoom(bool[] status)
    {
        for (int i = 0; i < status.Length; i++)
        {
            /*this currently just removes the door entirely, alter so it has a closed
             door and replaces with an open door perhaps?*/
            //m_doors[i].SetActive(!status[i]);

            if (status[i])
            {
                m_doors[i].SetActive(false);
            }
        }
        m_LightManager.SetActive(true);
    }

    public void SetLight(bool enable)
    {
        m_LightManager.SetActive(enable);
    }
}
