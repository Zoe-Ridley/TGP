using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState 
{
    public virtual void HandleMovement() { }

    public virtual void Update() { }
    
    private EnemyAI m_enemyAI;
    protected EnemyAI EnemyAI
    {
        get { return m_enemyAI; }
        set { m_enemyAI = value; }
    }
}
