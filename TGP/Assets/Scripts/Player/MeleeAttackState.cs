using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackState : EnemyState
{
    private float m_timeSinceLastAttack;

    public MeleeAttackState(EnemyAI enemyAI)
    {
        m_enemyAI = enemyAI;
        m_enemyAI.m_isMoving = false;
        m_timeSinceLastAttack = 0.0f;
    }

    public override void Update()
    {
        GameObject player = GameObject.Find("Player");
        float playerDistance = Vector3.Distance(player.transform.position, m_enemyAI.GetPosition());

        if (Time.fixedUnscaledTime > m_timeSinceLastAttack + m_enemyAI.GetAttackRate())
        {
            m_timeSinceLastAttack = Time.fixedUnscaledTime;

            if (playerDistance > m_enemyAI.GetAttackRange())
            {
                m_enemyAI.m_state = new RoamState(m_enemyAI);
            }
        }
    }
}
