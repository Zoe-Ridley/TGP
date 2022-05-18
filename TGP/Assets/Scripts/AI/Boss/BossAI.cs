using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : EnemyAI
{
    [SerializeField] protected GameObject m_minnion;

    [SerializeField] protected int m_SecondPhaseHealth;
    public int SecondPhaseHealth
    {
        get { return m_SecondPhaseHealth; }
        set { m_SecondPhaseHealth = value; }
    }

    [SerializeField] protected int m_ThirdPhaseHealth;
    public int ThirdPhaseHealth
    {
        get { return m_ThirdPhaseHealth; }
        set { m_ThirdPhaseHealth = value; }
    }

    new BossState m_state;
    public BossState State
    {
        get { return m_state; }
        set { m_state = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        m_state = new BossFirstPhase(this);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public void SpawnMinnion()
    {
        Instantiate(m_minnion, transform.position + new Vector3(1.0f, 0.0f, 0.0f), Quaternion.identity);
    }

    public void BoulderThrow()
    {
            
    }
}
