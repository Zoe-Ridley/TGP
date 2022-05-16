using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackState : MeleeEnemyState
{
    private float m_timeSinceLastAttack;

    public MeleeAttackState(MeleeEnemyAI enemyAI)
    {
        EnemyAI = enemyAI;
        EnemyAI.m_isMoving = false;
        m_timeSinceLastAttack = 0.0f;
    }

    public override void Update()
    {
        GameObject player = GameObject.Find("Player");
        float playerDistance = Vector3.Distance(player.transform.position, EnemyAI.GetCurrentPosition());

        if (Time.fixedUnscaledTime > m_timeSinceLastAttack + EnemyAI.GetAttackRate())
        {
            m_timeSinceLastAttack = Time.fixedUnscaledTime;

            if (playerDistance > EnemyAI.GetAttackRange())
            {
                EnemyAI.m_state = new RoamState(EnemyAI);
            }
        }
    }
}
