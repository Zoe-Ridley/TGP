using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : EnemyAI
{
    [SerializeField] protected GameObject m_minnion;
    [SerializeField] protected GameObject m_boulder;
    [SerializeField] protected GameObject m_stomp;
    [SerializeField] protected GameObject m_Barrier;
    [SerializeField] protected float m_throwSpeed;
    [SerializeField] protected float m_respawnRate;
    [SerializeField] protected float m_chargeSpeed;
    [SerializeField] protected float m_chargeRange;

    public float RespawnRate
    {
        get { return m_respawnRate; }
        set { m_respawnRate = value; }
    }

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

    [SerializeField] protected int m_stompRate;
    public int StompRate
    {
        get { return m_stompRate; }
        set { m_stompRate = value; }
    }

    private List<GameObject> m_spawnedEnemies;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        m_state = new SleepingState(this);
        m_spawnedEnemies = new List<GameObject>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public void SpawnMinnion()
    {
        GameObject tempRef = Instantiate(m_minnion, transform.position + new Vector3(1.0f, 0.0f, 0.0f), Quaternion.identity, transform.parent);
        m_spawnedEnemies.Add(tempRef);
    }

    public void BoulderThrow(Vector3 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        GameObject tempRef = Instantiate(m_boulder, transform.position, Quaternion.AngleAxis(angle, Vector3.forward));
        tempRef.GetComponent<Rigidbody2D>().AddForce(dir * m_throwSpeed, ForceMode2D.Impulse);
    }

    public void Stomp(Vector3 scale)
    {
       GameObject tempRef = Instantiate(m_stomp, transform.position, Quaternion.identity);
       tempRef.transform.localScale = scale;
    }

    public void ChargePlayer(Vector3 dir)
    {
        GetComponent<Rigidbody2D>().AddForce(dir * m_chargeSpeed, ForceMode2D.Impulse);
    }

    public void PutUpBarrier(Vector3 scale)
    {
        GameObject tempRef = Instantiate(m_Barrier, transform.position, Quaternion.identity);
        tempRef.transform.localScale = scale;
    }

    public void DestroyMinnions()
    {
        foreach(GameObject minnion in m_spawnedEnemies)
        {
            if (minnion)
                Destroy(minnion);
        }
    }
}
