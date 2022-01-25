using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestroy : MonoBehaviour
{
    [SerializeField] private float m_bulletLifetime = 5f;

    private void Update()
    {
        m_bulletLifetime -= Time.deltaTime;
        if (m_bulletLifetime < 0f)
        {
            Destroy(gameObject);
        }
    }
}
