using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    public GameObject[] m_walls; //0 Up, 1 Down, 2 Right, 3 Left
    public GameObject[] m_doors;
    [SerializeField] private GameObject m_CentreLight;
    [SerializeField] private GameObject[] m_CornerLights;

    public Vector2 RoomSize;

    //true = door closed
    //false = door open
    public void UpdateRoom(bool[] status)
    {
        for (int i = 0; i < status.Length; i++)
        {
            if (status[i])
            {
                m_doors[i].SetActive(false);
            }
        }
    }

    public void EnableLighting()
    {
        foreach (GameObject light in m_CornerLights)
        {
            light.SetActive(true);
        }

        m_CentreLight.SetActive(true);
    }
}
