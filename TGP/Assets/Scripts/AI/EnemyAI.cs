using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // Enemy info Variables
    [SerializeField] protected Material m_deathMaterial;
    [SerializeField] protected float m_targetRange;
    [SerializeField] protected float m_attackRange;
    [SerializeField] protected float m_attackRate;
    [SerializeField] protected float m_speed;

    // Item Data
    [SerializeField] ItemTable lootTable;
    [SerializeField] private GameObject[] PowerUp;
    public Transform m_SpawnPoint;

    // Dungeon Generated
    private DungeonGenerator m_generator;

    public float Speed
    {
        get { return m_speed; }
        set { m_speed = value; }
    }

    [SerializeField] protected int m_health;


    // The rigid body
    protected Rigidbody2D m_rb;

    // variables for dissolve
    protected float fade = 1.3f;
    protected bool fading = false;
    
    public bool m_isMoving { get; set; } = false;

    // position variables
    protected Vector3 m_startPosition;

    public Pathfinding m_pathFinder { get; protected set; }

    // enemy states
    public EnemyState m_state { get; set; }

    // Start is called before the first frame update
    public void Start()
    {
        m_startPosition = transform.position;
        m_pathFinder = transform.parent.GetComponent<RoomPathfindingSetup>().GetPathFinder();
        m_rb = GetComponent<Rigidbody2D>();
        m_generator = FindObjectOfType<DungeonGenerator>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (m_health <= 0)
        {
            if (!fading)
            {
                // Play the death animation and queue the object to be destroyed
                FindObjectOfType<AudioManager>().playAudio("EnemyDeath");
                Destroy(gameObject, fade);

                Item item = lootTable.GetLoot();
                Debug.Log(item.name);
                for (int i = 0; i < PowerUp.Length; i++)
                {
                    if (PowerUp[i].name == item.name)
                    {
                        Instantiate(PowerUp[i], m_SpawnPoint.position, Quaternion.identity);
                        Debug.Log(PowerUp[i].name);
                        break;
                    }
                }

                //Alter number of enemies
                Cell tempCell = m_generator.FindRoom(transform.position);
                tempCell.NumberOfEnemies--;

                if (tempCell.NumberOfEnemies == 0)
                {
                    m_generator.OpenRoom(GetComponentInParent<Transform>().position);
                }

                GetComponent<SpriteRenderer>().material = m_deathMaterial;
                gameObject.tag = "DeadEnemy";

                m_state = null;
                fading = true;
            }
            else
            { 
                GetComponent<SpriteRenderer>().material.SetFloat("_Fade", fade);
                fade -= Time.deltaTime;
            }

        }

        if (m_state != null)
            m_state.Update();
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerBullet")
        {
            m_health--;
        }
    }   

    /// <summary>
    /// <para>Using the pathfinder class the AI calculates a vector list to get to the destination </para>
    /// <para>The enemy goes through the vector list one by one to get to the destination </para>
    /// </summary>
    public void HandleMovement(List<Vector3> path)
    {
        if (path == null || path.Count <= 0)
        {
            return;
        }

        // Last node
        if (path.Count == 1)
        {
            if (Vector3.Distance(transform.position, path[0]) >= 0.5f)
            {
                Walk(path[0]);
            }
            else
            {
                path.RemoveAt(0);
                StopMoving();
            }
        }

        if (path != null)
        {
            // not the last node
            if (path.Count > 1)
            {
                if (Vector3.Distance(transform.position, path[0]) >= 0.5f)
                {
                    Walk(path[0]);
                }
                else
                {
                    path.RemoveAt(0);
                }
            }
        }
    }

    public void HandleMovement(List<Vector3> path, ref int index)
    {
        if (path == null || path.Count <= 0)
        {
            return;
        }

        // Ensure that we do not travel back to further node and the index is not more than the path count
        if (index + 1 < path.Count)
        {
            if (Vector3.Distance(transform.position, path[0]) > Vector3.Distance(transform.position, path[1]))
            {
                path.RemoveAt(0);
            }
        }

        // Keep looping as long as the index is not more than the path count
        if (index < path.Count)
        {
            if (Vector3.Distance(transform.position, path[index]) >= 0.5f)
            {
                Walk(path[index]);
            }
            else
            {
                index++;
            }
        }
        else
        {
            index = 0;
            path = null;
        }
    }

    /// <summary>
    /// <para> Move the enemy AI to a destination vector </para>
    /// <para> The function generates a direction vector and uses deltaTime and speed variable</para>
    /// </summary>
    protected void Walk(Vector3 destination)
    {
        Vector3 dir = (destination - transform.position).normalized;
        transform.position += (dir * m_speed * Time.deltaTime);
    }


    /// <summary>
    /// Generates a three dimensional ( z=0 ) normalized vector pointing in a random direction.
    /// </summary>
    protected static Vector3 GetRandomDir()
    {
        return new Vector3(UnityEngine.Random.Range(-1.0f, 1.0f), UnityEngine.Random.Range(-1.0f, 1.0f)).normalized;
    }

    protected void StopMoving()
    {
        m_isMoving = false;
    }

    // -----------------------------------
    // All the get functions for the class
    // -----------------------------------

    public float GetAttackRange()
    {
        return m_attackRange;
    }

    public float GetTargetRange()
    {
        return m_targetRange;
    }

    public float GetAttackRate()
    {
        return m_attackRate;
    }

    public Vector3 GetCurrentPosition()
    {
        return transform.position;
    }

    public Vector3 GetStartPosition()
    {
        return m_startPosition;
    }

    public void MoveByDir(Vector2 dir)
    {
        m_rb.AddForce(m_speed * dir);
    }
}
