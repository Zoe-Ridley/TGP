using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFirstPhase : BossState
{
    private float m_spawnTimer;
    private float m_TimeSinceLastAttack;

    private GameObject m_Player;

    public BossFirstPhase(BossAI bossAI)
    {
        BossAI = bossAI;
        m_spawnTimer = 0.0f;
        m_TimeSinceLastAttack = 0.0f;
        m_Player = GameObject.Find("Player");
    }

    public override void Update()
    {
        // Check if the boss should move to second phase
        if (BossAI.Health <= BossAI.SecondPhaseHealth)
        {
            BossAI.DestroyMinnions();
            BossAI.m_state = new BossSecondPhase(BossAI);
        }

        m_TimeSinceLastAttack += Time.deltaTime;
        m_spawnTimer += Time.deltaTime;

        // Spawn a minnion once the respawn rate time has passed
        if (m_spawnTimer >= BossAI.RespawnRate)
        {
            BossAI.SpawnMinnion();
            m_spawnTimer = 0.0f;
        }

        Debug.Log("FirstPhase");
        // Ranged attack 
        if (m_TimeSinceLastAttack >= BossAI.AttackRate)
        {
            Vector3 dir = (m_Player.transform.position - BossAI.transform.position).normalized;
            BossAI.BoulderThrow(dir);
            m_TimeSinceLastAttack = 0.0f;
        }
    }
}
