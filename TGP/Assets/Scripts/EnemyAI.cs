using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    

public class EnemyAI : MonoBehaviour 
{
    private enum EnemyState 
    { 
        CHASE,
        ATTACK,
        ROAM
    }


    // Enemy info Variables
    [SerializeField] private float m_speed;
    [SerializeField] private float m_targetRange;
    [SerializeField]private Vector2 m_roamRange;
    EnemyState m_currentState = EnemyState.ROAM;
    bool m_isMoving = false;
    float m_attackRange = 1.0f;

    // position variables
    private Vector3 m_direction;
    private Vector3 m_playerPosition;
    private Vector3 m_startPosition;
    private Vector3 m_roamingPosition;
    private Vector3 m_lastPosition;
    private List<Vector3> m_path;

    // Variables for pathfinding
    private Pathfinding m_pathFinder;

    // Start is called before the first frame update
    void Start()
    {
        m_startPosition = transform.position;
        m_roamingPosition = new Vector3(20f, 12f);
        m_direction = new Vector3(5.0f, 5.0f);
        m_pathFinder = new Pathfinding(100,100);
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.Find("Player");
        m_playerPosition = player.transform.position;

        switch (m_currentState)
        {
            case EnemyState.ROAM:
                if (!m_isMoving)
                {
                    m_path = Pathfinding.m_instance.FindPath(transform.position, GetRandomDestination());
                    m_isMoving = true;
                }
                else
                {
                    HandleMovement();
                }

                CheckForTarget();
                break;

            case EnemyState.CHASE:
                if (!m_isMoving)
                {
                    m_path = m_pathFinder.FindPath(transform.position, m_playerPosition);
                    m_isMoving = true;
                }
                else
                {
                    HandleMovement();
                }

                // start roaming if the enemy is out of range
                if (Vector3.Distance(m_playerPosition, transform.position) >= m_targetRange)
                {
                    m_currentState = EnemyState.ROAM;
                    m_isMoving = false;
                }

                // If the player is in range switch to attack
                if (Vector3.Distance(m_playerPosition, transform.position) <= m_attackRange)
                {
                    m_currentState = EnemyState.ATTACK;
                    m_isMoving = false;
                }
                break;

            case EnemyState.ATTACK:
                // attack the enemy
                Debug.Log("attacking");
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// <para>Using the pathfinder class the AI calculates a vector list to get to the destination </para>
    /// <para>The enemy goes through the vector list one by one to get to the destination </para>
    /// </summary>
    private void HandleMovement()
    {
        if (m_path == null || m_path.Count <= 0)
        {
            return;
        }

        // Last node
        if (m_path.Count == 1)
        {
            if (Vector3.Distance(transform.position, m_path[0]) >= 0.1f)
            {
                Walk(m_path[0]);
            }
            else
            {
                m_path.RemoveAt(0);
                StopMoving();
            }
        }

        if (m_path != null)
        {
            // not the last node
            if (m_path.Count > 1)
            {
                if (Vector3.Distance(transform.position, m_path[0]) >= 0.1f)
                {
                    Walk(m_path[0]);
                }
                else
                {
                    m_path.RemoveAt(0);
                }
            }
        }
    }

    /// <summary>
    /// <para> Move the enemy AI to a destination vector </para>
    /// <para> The function generates a direction vector and uses deltaTime and speed variable</para>
    /// </summary>
    private void Walk(Vector3 destination)
    {
        Vector3 dir = (destination - transform.position).normalized;

        transform.position += m_speed * dir * Time.deltaTime;
    }

    /// <summary>
    /// Generate a random destinatino for the AI between the min and max range
    /// </summary>
    private Vector3 GetRandomDestination()
    {
        return m_startPosition + GetRandomDir() * Random.Range(m_roamRange.x, m_roamRange.y);
    }


    /// <summary>
    /// Generates a three dimensional ( z=0 ) normalized vector pointing in a random direction.
    /// </summary>
    public static Vector3 GetRandomDir()
    {
        return new Vector3(UnityEngine.Random.Range(-1.0f, 1.0f), UnityEngine.Random.Range(-1.0f, 1.0f)).normalized;
    }

    /// <summary>
    /// Check if the enemy player is within range and if they switch the current state to CHASE
    /// </summary>
    /// <returns></returns>
    private void CheckForTarget()
    { 
        if (Vector3.Distance(transform.position, m_playerPosition) <= m_targetRange)
        {
            // Player is within range
            m_currentState = EnemyState.CHASE;
            m_isMoving = false;
        }
    }

    private void StopMoving()
    {
        m_isMoving = false;
        m_path = null;
    }
}
