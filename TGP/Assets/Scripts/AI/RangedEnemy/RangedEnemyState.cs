using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyState : EnemyState
{
    protected RangedEnemyAI m_enemyAI;
    public new RangedEnemyAI EnemyAI
    {
        get { return m_enemyAI; }
        set { m_enemyAI = value; }
    }
}
