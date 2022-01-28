using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoamState : EnemyState
{
    private List<Vector3> path;
    public RoamState(EnemyAI enemyAI)
    {
        m_enemyAI = enemyAI;
        enemyAI.m_isMoving = false;
        path = null;
    }

    public override void Update() 
    {
        GameObject player = GameObject.Find("Player");
        float distanceToPlayer = Vector3.Distance(player.transform.position, m_enemyAI.GetPosition());

        // if the enemy AI does not have a path generate a random path
        if (!m_enemyAI.m_isMoving)
        {
            path = m_enemyAI.m_pathFinder.FindPath(m_enemyAI.GetPosition(), m_enemyAI.GetRandomDestination());
            m_enemyAI.m_isMoving = true;
        }
        else
        {
            // once we have a path handle the movement logic to the path
            m_enemyAI.HandleMovement(path);
        }

        // if the enemy is withing range switch to chasing
        if (distanceToPlayer < m_enemyAI.GetTargetRange())
        {
            m_enemyAI.m_state = new ChaseState(m_enemyAI);
        }
    }
}
