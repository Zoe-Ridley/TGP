using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackState : RangedEnemyState
{
    public RangedAttackState(RangedEnemyAI enemyAI)
    {
        m_enemyAI = enemyAI;
    }
}
