using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode 
{

    private Grid<PathNode> m_grid;
    public int x;
    public int y;

    public int m_gCost;
    public int m_hCost;
    public int m_fCost;

    public bool m_isWalkable;
    public PathNode m_parentNode;

    public PathNode(Grid<PathNode> grid, int x, int y, bool walkable = true) 
    {
        m_grid = grid;
        this.x = x;
        this.y = y;
        m_isWalkable = walkable;
    }

    /// <summary>
    /// Calculate the F cost by summing up the gCost and the hCost
    /// </summary>
    public void CalculateFCost() 
    {
        m_fCost = m_gCost + m_hCost;
    }

    /// <summary>
    /// Set if the tile is walkable
    /// </summary>
    public void SetIsWalkable(bool isWalkable) 
    {
        m_isWalkable = isWalkable;
        m_grid.TriggerGridObjectChanged(x, y);
    }

    /// <summary>
    /// Converts the x and y coordinates to a string: "x,y"
    /// </summary>
    public override string ToString() 
    {
        return x + "," + y;
    }

}
