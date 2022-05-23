using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Attacks
{
    CHARGE,
    STOMP,
    THROW,
    PASSIVE
}

public class BossSecondPhase : BossState
{
    private float m_stompTimer;
    private float m_chargeTimer;
    private float m_chargeRate = 5.0f;
    private Vector3 m_chargeDistance, m_InitialPosition;
    Attacks m_currentAttack;

    public BossSecondPhase(BossAI bossAI, GameObject player)
    {
        BossAI = bossAI;
        m_player = player;
    }

    public override void Update()
    {
        if (BossAI.Health <= BossAI.ThirdPhaseHealth)
        {
            BossAI.m_state = new BossThirdPhase(BossAI);
        }

        // Update timer
        m_stompTimer += Time.deltaTime;
        m_chargeTimer += Time.deltaTime;

        m_currentAttack = Attacks.PASSIVE;

        if (m_stompTimer >= BossAI.StompRate)
        {
            m_currentAttack = Attacks.STOMP;
        }

        if (m_chargeTimer >= m_chargeRate)
        {
            m_currentAttack = Attacks.CHARGE;
        }

        switch (m_currentAttack)
        {
            case Attacks.CHARGE:
            {
                Vector3 dir = (m_player.transform.position - BossAI.transform.position).normalized;
                BossAI.ChargePlayer(dir);
                m_chargeTimer = 0.0f;
                break;
            }
            case Attacks.STOMP:
            {
                BossAI.Stomp(new Vector3(12.0f, 12.0f, 1.0f));
                m_stompTimer = 0.0f;
                break;
            }
            case Attacks.THROW:
            {
                break;
            }
            case Attacks.PASSIVE:
            {
                break;
            }
        }
    }
}
