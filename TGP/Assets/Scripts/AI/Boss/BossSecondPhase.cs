using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSecondPhase : BossState
{
    public BossSecondPhase(BossAI bossAI)
    {
        BossAI = bossAI;
    }

    public override void Update()
    {
        if (BossAI.Health <= BossAI.ThirdPhaseHealth)
        {
            BossAI.m_state = new BossThirdPhase(BossAI);
        }
    }
}
