using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossThirdPhase : BossState
{
    private float m_stompTimer;
    private float m_ThrowTimer;
    private float m_BlinkTimer;

    private float m_ThrowRate = 2.0f;
    private float m_BlinkRate = 3.5f;

    Attacks m_currentAttack;
    private List<GameObject> m_BlinkPoints;

    public BossThirdPhase(BossAI bossAI, GameObject player)
    {
        BossAI = bossAI;
        m_player = player;

        GameObject parent = GameObject.Find("BlinkPoints");
        m_BlinkPoints = new List<GameObject>();

        for (int i = 0; i < parent.transform.childCount; i++)
        {
            m_BlinkPoints.Add(parent.transform.GetChild(i).gameObject);
        }
    }

    public override void Update()
    {
        // Update timer
        m_stompTimer += Time.deltaTime;
        m_ThrowTimer += Time.deltaTime;
        m_BlinkTimer += Time.deltaTime;

        m_currentAttack = Attacks.PASSIVE;

        if (m_ThrowTimer >= m_ThrowRate)
        {
            m_currentAttack = Attacks.THROW;
        }
        else if (m_BlinkTimer >= m_BlinkRate)
        {
            m_currentAttack = Attacks.BLINK;
        }

        switch (m_currentAttack)
        {
            case Attacks.THROW:
                {
                    BossAI.m_animator.SetTrigger("isThrowing");
                    BossAI.BoulderThrow(m_player.transform.position);
                    m_ThrowTimer = 0.0f;
                    break;
                }
            case Attacks.BLINK:
                {
                    BossAI.m_animator.SetTrigger("isMoving");
                    int index = 0;
                    float lowestDistance = float.MaxValue;

                    for (int i = 0; i < m_BlinkPoints.Count; i++)
                    {
                        if (Vector3.Distance(m_BlinkPoints[i].transform.position, m_player.transform.position) <= lowestDistance)
                        {
                            index = i;
                            lowestDistance = Vector3.Distance(m_BlinkPoints[i].transform.position, m_player.transform.position);
                        }
                    }

                    BossAI.GetComponent<Rigidbody2D>().MovePosition(m_BlinkPoints[index].transform.position);
                    BossAI.transform.position = m_BlinkPoints[index].transform.position;
                    BossAI.Stomp(new Vector3(12.0f, 12.0f, 1.0f));
                    m_BlinkTimer = 0.0f;
                    break;
                }
            case Attacks.PASSIVE:
                {
                    break;
                }
        }
    }
}
