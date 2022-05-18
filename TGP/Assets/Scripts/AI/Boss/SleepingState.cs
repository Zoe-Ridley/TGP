using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepingState : BossState
{
    bool playerDetected;
    public SleepingState(BossAI bossAI)
    {
        BossAI = bossAI;
        playerDetected = false;
    }

    public override void Update()
    {
        if (playerDetected)
        {

        }
    }
}
