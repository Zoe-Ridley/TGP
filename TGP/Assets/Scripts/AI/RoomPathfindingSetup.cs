using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPathfindingSetup : MonoBehaviour
{
    [SerializeField] int GridWidth;
    [SerializeField] int GridHeight;
    [SerializeField] int TileSize;
    [SerializeField] Vector3 OriginPosition;

    Pathfinding m_PathFinder;
    Grid<PathNode> m_grid;

    // Must be set to awake to ensure it is called before Enemy script
    // Note: Awake functions are guranteed to be called before start functions
    void Awake()
    {
        Vector3 vec3 = new Vector3(transform.position.x, transform.position.y);

        m_grid = new Grid<PathNode>(GridWidth, GridHeight, TileSize, OriginPosition, (Grid<PathNode> g, int x, int y) 
            => new PathNode(g, x, y));

        m_PathFinder = new Pathfinding(m_grid);
    }

    public Grid<PathNode> GetGrid()
    {
        return m_grid;    
    }

    public Pathfinding GetPathFinder()
    {
        return m_PathFinder;
    }
}
