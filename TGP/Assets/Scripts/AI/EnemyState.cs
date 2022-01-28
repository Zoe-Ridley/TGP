using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState 
{
    public virtual void HandleMovement() { }

    public virtual void Update() { }

    protected EnemyAI m_enemyAI;
    protected Vector3 m_playerPosition;
}
