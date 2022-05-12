using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Should be used for the Melee enemy starting state
/// </summary>

public class RoamState : MeleeEnemyState
{
    private List<Vector3> path;
    public RoamState(MeleeEnemyAI enemyAI)
    {
        EnemyAI = enemyAI;
        enemyAI.m_isMoving = false;
        path = null;
    }

    public override void Update() 
    {
        GameObject player = GameObject.Find("Player");
        float distanceToPlayer = Vector3.Distance(player.transform.position, EnemyAI.GetCurrentPosition());

        // if the enemy is withing range switch to chasing
        if (distanceToPlayer < EnemyAI.GetTargetRange())
        {
            EnemyAI.m_state = new ChaseState(EnemyAI);
        }

        // if the enemy AI does not have a path generate a random path
        if (!EnemyAI.m_isMoving)
        {
            path = EnemyAI.m_pathFinder.FindPath(EnemyAI.GetCurrentPosition(), EnemyAI.GetRandomDestination());
            EnemyAI.m_isMoving = true;
        }
        else
        {
            // once we have a path handle the movement logic to the path
            EnemyAI.HandleMovement(path);
        }

    }
}
