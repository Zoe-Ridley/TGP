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

    // Start is called before the first frame update
    void Start()
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
