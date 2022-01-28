using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyAI : MonoBehaviour
{
    // Enemy info Variables
    [SerializeField] private float m_speed;
    [SerializeField] private float m_targetRange;
    [SerializeField] private float m_attackRange = 1.0f;
    [SerializeField] private float m_attackRate = 1.0f;
    [SerializeField] private Vector2 m_roamRange;

    public bool m_isMoving { get; set; } = false;

    // position variables
    private Vector3 m_playerPosition;
    private Vector3 m_startPosition;

    public Pathfinding m_pathFinder { get; private set; }

    // enemy states
    public EnemyState m_state { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        m_startPosition = transform.position;
        m_pathFinder = transform.parent.gameObject.GetComponent<RoomPathfindingSetup>().GetPathFinder();
        m_state = new RoamState(this);
    }

    // Update is called once per frame
    void Update()
    {
        m_state.Update();
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
            if (Vector3.Distance(transform.position, path[0]) >= 0.1f)
            {
                Walk(path[0]);
                Debug.Log(path[0].ToString());
            }
            else
            {
                path.RemoveAt(0);
            }
        }

        if (path != null)
        {
            // not the last node
            if (path.Count > 1)
            {
                if (Vector3.Distance(transform.position, path[0]) >= 0.1f)
                {
                    Walk(path[0]);
                    Debug.Log(path[0].ToString());
                }
                else
                {
                    path.RemoveAt(0);
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
    public Vector3 GetRandomDestination()
    {
        return m_startPosition + GetRandomDir() * Random.Range(m_roamRange.x, m_roamRange.y);
    }


    /// <summary>
    /// Generates a three dimensional ( z=0 ) normalized vector pointing in a random direction.
    /// </summary>
    private static Vector3 GetRandomDir()
    {
        return new Vector3(UnityEngine.Random.Range(-1.0f, 1.0f), UnityEngine.Random.Range(-1.0f, 1.0f)).normalized;
    }

    /// <summary>
    /// Check if the enemy player is within range and if they switch the current state to CHASE
    /// </summary>
    /// <returns></returns>
    public void CheckForTarget()
    { 
        if (Vector3.Distance(transform.position, m_playerPosition) <= m_targetRange)
        {
            // Player is within range
            m_isMoving = false;
        }
    }

    private void StopMoving()
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

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
