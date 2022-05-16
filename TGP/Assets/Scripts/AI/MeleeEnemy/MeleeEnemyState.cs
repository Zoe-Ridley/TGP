using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyState : EnemyState
{
    protected MeleeEnemyAI m_enemyAI;
    public new MeleeEnemyAI EnemyAI
    {
        get { return m_enemyAI; }
        set { m_enemyAI = value; }
    }
}
