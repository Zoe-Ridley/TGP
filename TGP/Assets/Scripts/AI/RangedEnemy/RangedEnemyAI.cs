using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyAI : EnemyAI
{
    [SerializeField] protected float m_firingForce;
    [SerializeField] protected GameObject m_bulletPrefab;
    [SerializeField] protected float m_dangerDistance;

    private Animator m_animator;

    public float DangerDistance
    {
        get { return m_dangerDistance; }
        set { m_dangerDistance = value; }
    }

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

        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public void Attack(Vector3 dir)
    {
        m_animator.SetBool("isAttacking", true);

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        GameObject tempRef = Instantiate(m_bulletPrefab, transform.position, Quaternion.AngleAxis(angle, Vector3.forward));
        tempRef.GetComponent<Rigidbody2D>().AddForce(dir * m_firingForce, ForceMode2D.Impulse);
    }
}
