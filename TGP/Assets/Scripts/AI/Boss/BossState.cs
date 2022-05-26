using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossState : EnemyState
{
    BossAI m_BossAI;
    protected GameObject m_player;

    public BossAI BossAI
    {
        get { return m_BossAI; }
        set { m_BossAI = value; }
    }
}
