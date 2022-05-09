using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : EnemyState
{
    private List<Vector3> path;
    private int index;
    public ChaseState(EnemyAI enemyAI)
    {
        m_enemyAI = enemyAI;
        m_enemyAI.m_isMoving = false;
        path = null;
        index = 0;
    }

    public override void Update()
    {
        GameObject player = GameObject.Find("Player");
        float playerDistance = Vector3.Distance(player.transform.position, m_enemyAI.GetPosition());

        // if the enemy AI does not have a path generate a random path
        path = m_enemyAI.m_pathFinder.FindPath(m_enemyAI.GetPosition(), player.transform.position);
        m_enemyAI.HandleMovement(path, ref index);


        // start roaming if the enemy is out of range
        if (playerDistance >= m_enemyAI.GetTargetRange())
        {
            m_enemyAI.m_state = new RoamState(m_enemyAI);
        }

        // If the player is in range switch to attack
        if (playerDistance <= m_enemyAI.GetAttackRange())
        {
            m_enemyAI.m_state = new MeleeAttackState(m_enemyAI);
        }
    }
}
