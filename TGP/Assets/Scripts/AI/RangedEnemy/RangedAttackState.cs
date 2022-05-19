using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackState : RangedEnemyState
{
    Transform playerTransform;
    private float m_timeSinceLastAttack;

    public RangedAttackState(RangedEnemyAI enemyAI)
    {
        m_enemyAI = enemyAI;
        m_timeSinceLastAttack = 0.0f;
        playerTransform = GameObject.Find("Player").transform;
    }

    public override void Update()
    {
        float playerDistance = Vector3.Distance(playerTransform.position, m_enemyAI.transform.position);
        Vector3 playerDir = playerTransform.position - m_enemyAI.transform.position;
        m_timeSinceLastAttack += Time.deltaTime;

        // if the player leaves the target range go back to scouting
        if (playerDistance > m_enemyAI.GetTargetRange())
        {
            m_enemyAI.m_state = new ScoutState(m_enemyAI);
        }
        else if (playerDistance < m_enemyAI.GetAttackRange() && playerDistance > m_enemyAI.DangerDistance)
        {
            if (m_timeSinceLastAttack >= m_enemyAI.GetAttackRate())
            { 
                m_enemyAI.Attack(playerDir);
                m_timeSinceLastAttack = 0.0f;
            }
        }
        else
        {
            // get the opposite direction of the player
            Vector3 oppositeDir = playerDir * -1;
            oppositeDir.Normalize();

            m_enemyAI.MoveByDir(oppositeDir);

            if (m_timeSinceLastAttack >= m_enemyAI.GetAttackRate())
            {
                m_enemyAI.Attack(playerDir);
                m_timeSinceLastAttack = 0.0f;
            }
        }
    }
}
