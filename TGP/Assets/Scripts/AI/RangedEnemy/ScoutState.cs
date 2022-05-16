using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Should be used for ranged enemy starting state
/// </summary>

public class ScoutState : RangedEnemyState
{
    private List<Vector3> path;
    private Vector2 roamPos;

    public ScoutState(RangedEnemyAI enemyAI)
    {
        m_enemyAI = enemyAI;
        m_enemyAI.m_isMoving = false;
        path = null;

        GridSide gridSide = m_enemyAI.m_pathFinder.GetGrid().GetGridSide(m_enemyAI.GetCurrentPosition());

        switch (gridSide)
        {
            case GridSide.TopRight:
                roamPos = m_enemyAI.GetCurrentPosition();
                roamPos.x -= m_enemyAI.ScoutRange.x;
                roamPos.y -= m_enemyAI.ScoutRange.y;
                break;
            case GridSide.TopLeft:
                roamPos = m_enemyAI.GetCurrentPosition();
                roamPos.x += m_enemyAI.ScoutRange.x;
                roamPos.y -= m_enemyAI.ScoutRange.y;
                break;
            case GridSide.BottomRight:
                roamPos = m_enemyAI.GetCurrentPosition();
                roamPos.x -= m_enemyAI.ScoutRange.x;
                roamPos.y += m_enemyAI.ScoutRange.y;
                break;
            case GridSide.BottomLeft:
                roamPos = m_enemyAI.GetCurrentPosition();
                roamPos.x += m_enemyAI.ScoutRange.x;
                roamPos.y += m_enemyAI.ScoutRange.y;
                break;
        }
    }

    public override void Update()
    {
        GameObject player = GameObject.Find("Player");
        float distanceToPlayer = Vector3.Distance(player.transform.position, m_enemyAI.GetCurrentPosition());

        if (distanceToPlayer <= m_enemyAI.GetTargetRange())
        {
            m_enemyAI.m_state = new RangedAttackState(m_enemyAI);
        }
        
        if (!m_enemyAI.m_isMoving)
        {
            if (Vector3.Distance(m_enemyAI.GetCurrentPosition(), m_enemyAI.GetStartPosition()) <= 1.5f)
            {
                path = m_enemyAI.m_pathFinder.FindPath(m_enemyAI.GetCurrentPosition(), roamPos);
                m_enemyAI.m_isMoving = true;
            }
            else
            {
                path = m_enemyAI.m_pathFinder.FindPath(m_enemyAI.GetCurrentPosition(), m_enemyAI.GetStartPosition());
                m_enemyAI.m_isMoving = true;
            }
        }
        else
        {
            m_enemyAI.HandleMovement(path);
        }
    }
}
