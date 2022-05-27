using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossThirdPhase : BossState
{
    public BossThirdPhase(BossAI bossAI)
    {
        BossAI = bossAI;

        bossAI.ThirdPhase();
    }

    public override void Update()
    {
        
    }
}
