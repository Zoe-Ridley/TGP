using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : MeleeEnemyState
{
    private List<Vector3> path;
    private int index;

    protected Animator m_animator;

    public ChaseState(MeleeEnemyAI enemyAI)
    {
        EnemyAI = enemyAI;
        EnemyAI.m_isMoving = false;
        path = null;
        index = 0;
    }

    public void Start()
    {
      //  m_animator = GetComponent<Animator>();
    }

    public override void Update()
    {
        GameObject player = GameObject.Find("Player");
        float playerDistance = Vector3.Distance(player.transform.position, EnemyAI.GetCurrentPosition());

        // if the enemy AI does not have a path generate a random path
        path = EnemyAI.m_pathFinder.FindPath(EnemyAI.GetCurrentPosition(), player.transform.position);
        EnemyAI.HandleMovement(path, ref index);


        // start roaming if the enemy is out of range
        if (playerDistance >= EnemyAI.GetTargetRange())
        {
            EnemyAI.m_state = new RoamState(EnemyAI);
        }

        // If the player is in range switch to attack
        if (playerDistance <= EnemyAI.GetAttackRange())
        {
            EnemyAI.m_state = new MeleeAttackState(EnemyAI);
        }
    }
}
