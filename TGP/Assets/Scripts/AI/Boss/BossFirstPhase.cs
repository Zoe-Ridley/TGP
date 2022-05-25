using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFirstPhase : BossState
{
    private float m_spawnTimer;
    private float m_timeSinceLastAttack;

    public BossFirstPhase(BossAI bossAI)
    {
        BossAI = bossAI;
        m_player = GameObject.Find("Player");
    }

    public override void Update()
    {
        // Check if the boss should move to second phase
        if (BossAI.Health <= BossAI.SecondPhaseHealth)
        {
            BossAI.DestroyMinnions();
            BossAI.PutUpBarrier(new Vector3(5.0f, 5.0f, 1.0f));
            BossAI.m_state = new BossSecondPhase(BossAI, m_player);
        }

        // Update timers
        m_timeSinceLastAttack += Time.deltaTime;
        m_spawnTimer += Time.deltaTime;

        // Spawn a minnion once the respawn rate time has passed
        if (m_spawnTimer >= BossAI.RespawnRate)
        {
            BossAI.SpawnMinnion();
            m_spawnTimer = 0.0f;
        }

        // Ranged attack
        if (m_timeSinceLastAttack >= BossAI.AttackRate)
        {
            Vector3 dir = m_player.transform.position - BossAI.transform.position;
            BossAI.FireBullet(dir.normalized);
            m_timeSinceLastAttack = 0.0f;
        }
    }
}
