using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : MonoBehaviour
{
    private bool m_hasDestination = false;
    private Vector3 m_destination;

    private float m_destructTime = float.MaxValue;
    private float m_destructCounter = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        m_destructCounter += Time.deltaTime;

        if (m_hasDestination)
        {
            if (Vector3.Distance(transform.position, m_destination) <= 0.5f)
            {
                Destroy(transform.gameObject);
            }
        }

        if (m_destructCounter >= m_destructTime)
        {
            Destroy(transform.gameObject);
        }
    }

    public void SetDestination(Vector3 destination)
    {
        m_hasDestination = true;
        m_destination = destination;
    }

    public void SetTimer(float timer)
    {
        m_destructTime = timer;
    }
}
