using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFirstPhase : BossState
{
    private float m_spawnTimer;
    private float m_respawnRate;
    private float m_TimeSinceLastAttack;

    public BossFirstPhase(BossAI bossAI)
    {
        BossAI = bossAI;
        m_spawnTimer = 0.0f;
        m_TimeSinceLastAttack = 0.0f;
    }

    public override void Update()
    {
        // Check if the boss should move to second phase
        if (BossAI.Health <= BossAI.SecondPhaseHealth)
        {
            BossAI.m_state = new BossSecondPhase(BossAI);
        }

        // Spawn a minnion once the respawn rate time has passed
        if (m_spawnTimer >= m_respawnRate)
        {
            BossAI.SpawnMinnion();
            m_spawnTimer = 0.0f;
        }

        // Ranged attack 
        if (m_TimeSinceLastAttack >= EnemyAI.GetAttackRate())
        {
            BossAI.BoulderThrow();
        }
    }
}
