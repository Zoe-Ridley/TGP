using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyPrefab : MonoBehaviour
{
    [SerializeField] private GameObject m_weaponPrefab;

    private float m_afterTime = 0.5f;

    void Update()
    {
        if (m_afterTime <= 0f)
        {
            Destroy(m_weaponPrefab);
        }
    }
}
