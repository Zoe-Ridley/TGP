using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : EnemyAI
{
    [SerializeField] protected GameObject m_firstPhaseBar;
    [SerializeField] protected GameObject m_minnion;
    [SerializeField] protected GameObject m_bullet;
    [SerializeField] protected GameObject m_boulder;
    [SerializeField] protected GameObject m_stomp;
    [SerializeField] protected GameObject m_Barrier;
    [SerializeField] protected float m_throwSpeed;
    [SerializeField] protected float m_bulletSpeed;
    [SerializeField] protected float m_respawnRate;

    public Animator m_animator;
    public int m_maxHealth;

    public float RespawnRate
    {
        get { return m_respawnRate; }
        set { m_respawnRate = value; }
    }

    [SerializeField] protected int m_SecondPhaseHealth;
    [SerializeField] protected GameObject m_SecondPhaseBar;

    public int SecondPhaseHealth
    {
        get { return m_SecondPhaseHealth; }
        set { m_SecondPhaseHealth = value; }

    }

    [SerializeField] protected int m_ThirdPhaseHealth;
    [SerializeField] protected GameObject m_ThirdPhaseBar;

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

    [SerializeField] protected float m_throwRate;
    public float ThrowRate
    {
        get { return m_throwRate; }
        set { m_throwRate = value; }
    }

    private List<GameObject> m_spawnedEnemies;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        m_state = new SleepingState(this);
        m_spawnedEnemies = new List<GameObject>();
        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if(m_health <= 0)
        {
            m_animator.SetTrigger("isDead");
        }
    }

    public void SpawnMinnion()
    {
        GameObject tempRef = Instantiate(m_minnion, transform.position + new Vector3(1.0f, 0.0f, 0.0f), Quaternion.identity, transform.parent);
        m_spawnedEnemies.Add(tempRef);
    }

    public void BoulderThrow(Vector3 destination)
    {
        Vector3 dir = destination - transform.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        GameObject tempRef = Instantiate(m_boulder, transform.position, Quaternion.AngleAxis(angle, Vector3.forward));

        tempRef.GetComponent<Rigidbody2D>().AddForce(dir * m_throwSpeed, ForceMode2D.Impulse);

        GameObject tempRefHB = Instantiate(m_stomp, transform.position, Quaternion.identity);

        Vector3 scale = new Vector3(5.0f, 5.0f);
        float fillSpeed = Vector3.Distance(destination, transform.position) / tempRef.GetComponent<Rigidbody2D>().velocity.magnitude;
        fillSpeed = scale.x / fillSpeed;

        tempRef.GetComponent<Boulder>().SetTimer(Vector3.Distance(destination, transform.position) / tempRef.GetComponent<Rigidbody2D>().velocity.magnitude);

        tempRefHB.GetComponent<Hitbox>().Setup(scale, fillSpeed, destination);
    }

    public void FireBullet(Vector3 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        GameObject tempRef = Instantiate(m_bullet, transform.position, Quaternion.AngleAxis(angle, Vector3.forward));
        tempRef.GetComponent<Rigidbody2D>().AddForce(dir * m_bulletSpeed, ForceMode2D.Impulse);
    }

    public void Stomp(Vector3 scale)
    {
       GameObject tempRef = Instantiate(m_stomp, transform.position, Quaternion.identity);
       tempRef.transform.localScale = scale;
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

    public void FirstPhase()
    {
        m_firstPhaseBar.SetActive(true);
    }

    public void SecondPhase()
    {
        m_firstPhaseBar.SetActive(false);
        m_SecondPhaseBar.SetActive(true);
    }

    public void ThirdPhase()
    {
        m_SecondPhaseBar.SetActive(false);
        m_ThirdPhaseBar.SetActive(true);
    }
}
