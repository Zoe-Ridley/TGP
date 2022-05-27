using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Attacks
{
    CHARGE,
    STOMP,
    THROW,
    PASSIVE
}

public class BossSecondPhase : BossState
{
    private float m_stompTimer;
    private float m_ThrowTimer;
    private float m_ThrowRate = 2.0f;
    Attacks m_currentAttack;

    public Animator m_animator;

    public BossSecondPhase(BossAI bossAI, GameObject player)
    {
        BossAI = bossAI;

        bossAI.SecondPhase();

        m_player = player;
    }

    public void Start()
    {
      m_animator = BossAI.GetComponent<Animator>();
    }

    public override void Update()
    {

        if (BossAI.Health <= BossAI.ThirdPhaseHealth)
        {
            BossAI.m_state = new BossThirdPhase(BossAI);
        }

        // Update timer
        m_stompTimer += Time.deltaTime;
        m_ThrowTimer += Time.deltaTime;

        Debug.Log(m_ThrowTimer);

        m_currentAttack = Attacks.PASSIVE;

        if (m_stompTimer >= BossAI.StompRate)
        {
            m_currentAttack = Attacks.STOMP;
        }
        else if (m_ThrowTimer >= m_ThrowRate)
        {
            m_currentAttack = Attacks.THROW;
        }

        switch (m_currentAttack)
        {
            case Attacks.STOMP:
            {
                BossAI.Stomp(new Vector3(12.0f, 12.0f, 1.0f));
                m_stompTimer = 0.0f;

                // animations.
                BossAI.m_animator.SetTrigger("isStomping");
                    BossAI.m_animator.SetBool("isIdle", false);

                    break;
            }
            case Attacks.THROW:
            {
                    BossAI.BoulderThrow(m_player.transform.position);
                m_ThrowTimer = 0.0f;

                // animations.
                BossAI.m_animator.SetTrigger("isThrowing");
                    BossAI.m_animator.SetBool("isIdle", false);

                    break;
            }
            case Attacks.PASSIVE:
            {
              //   BossAI.m_animator.SetBool("isIdle", true);
                    break;
            }
        }
    }
}
