using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyAI : EnemyAI
{
    [SerializeField] protected Vector2 m_roamRange;
    protected Vector2 RoamRange
    {
        get { return m_roamRange; }
        set { m_roamRange = value; }
    }

    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();
        m_state = new RoamState(this);
    }

    // Update is called once per frame
    public new void Update()
    {
        base.Update();
    }

    /// <summary>
    /// Generate a random destination for the AI between the min and max range
    /// </summary>
    public Vector3 GetRandomDestination()
    {
        return m_startPosition + GetRandomDir() * Random.Range(m_roamRange.x, m_roamRange.y);
    }
}
