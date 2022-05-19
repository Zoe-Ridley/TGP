using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepingState : BossState
{
    private GameObject m_Player;
    public SleepingState(BossAI bossAI)
    {
        BossAI = bossAI;
        m_Player = GameObject.Find("Player");
    }

    public override void Update()
    {
        Debug.Log("Sleeping");
        if (Vector3.Distance(m_Player.transform.position, BossAI.transform.position) <= BossAI.GetTargetRange())
        {
            BossAI.m_state = new BossFirstPhase(BossAI);
        }
    }
}
