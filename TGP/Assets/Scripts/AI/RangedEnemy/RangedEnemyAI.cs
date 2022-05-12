using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyAI : EnemyAI
{
    [SerializeField] protected Vector2 m_scoutRange;
    public Vector2 ScoutRange
    {
        get { return m_scoutRange; }
        set { m_scoutRange = value; }
    }

    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();
        m_state = new ScoutState(this);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
