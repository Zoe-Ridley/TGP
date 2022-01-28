using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding 
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    private Grid<PathNode> m_grid;
    private List<PathNode> m_openList;
    private List<PathNode> m_closedList;

    public Pathfinding(int width, int height)
    {
        m_grid = new Grid<PathNode>(width, height, 1f, Vector3.zero, (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y));
    }

    public Pathfinding(Grid<PathNode> grid)
    {
        m_grid = grid;
    }

    /// <summary>
    /// Returns the grid for pathfinding
    /// </summary>
    public Grid<PathNode> GetGrid() 
    {
        return m_grid;
    }

    /// <summary>
    /// <para>Finds the path from one vector to another </para>
    /// <para>This overload returns a list of vector3's which can make movement easier for the character </para>
    /// <para>Returns null and prints an error if the path was not found </para>
    /// </summary>
    public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition) 
    {
        // convert the start and end world position to X and Y format
        m_grid.GetXY(startWorldPosition, out int startX, out int startY);
        m_grid.GetXY(endWorldPosition, out int endX, out int endY);

        List<PathNode> path = FindPath(startX, startY, endX, endY);
        if (path == null) 
        {
            Debug.Log("returned path was null in Vector3 FindPath - " + this.ToString());
            return null;
        }
        else 
        {
            List<Vector3> vectorPath = new List<Vector3>();

            foreach (PathNode pathNode in path) {
                vectorPath.Add(new Vector3(pathNode.x, pathNode.y) * m_grid.GetCellSize() + Vector3.one * m_grid.GetCellSize() * .5f);
            }

            return vectorPath;
        }
    }

    /// <summary>
    /// <para>Finds the path from one point to another </para>
    /// <para>Returns a list of pathnodes, the list does not need to be reversed as the function does it for you </para>
    /// <para>If the function fails it prints out an error message and returns null </para>
    /// </summary>
    public List<PathNode> FindPath(int startX, int startY, int endX, int endY) {
        PathNode startNode = m_grid.GetGridObject(startX, startY);
        PathNode endNode = m_grid.GetGridObject(endX, endY);

        if (startNode == null || endNode == null) 
        {
            Debug.Log("Error: start or end node is null" + this.ToString());
            return null;
        }

        m_openList = new List<PathNode> { startNode };
        m_closedList = new List<PathNode>();

        for (int x = 0; x < m_grid.GetWidth(); x++) {
            for (int y = 0; y < m_grid.GetHeight(); y++) {

                PathNode pathNode = m_grid.GetGridObject(x, y);
                pathNode.m_gCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode.m_parentNode = null;
            }
        }

        startNode.m_gCost = 0;
        startNode.m_hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        // Keep looping while there are still nodes in the open list
        while (m_openList.Count > 0) 
        {
            PathNode currentNode = GetLowestFcost(m_openList);
            if (currentNode == endNode) 
            {
                return CalculatePath(endNode);
            }

            m_openList.Remove(currentNode);
            m_closedList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeighbourList(currentNode)) 
            {
                if (m_closedList.Contains(neighbourNode)) 
                    continue;

                if (!neighbourNode.m_isWalkable) 
                {
                    m_closedList.Add(neighbourNode);
                    continue;
                }

                int tentativeGCost = currentNode.m_gCost + CalculateDistanceCost(currentNode, neighbourNode);

                if (tentativeGCost < neighbourNode.m_gCost) 
                {
                    neighbourNode.m_parentNode = currentNode;
                    neighbourNode.m_gCost = tentativeGCost;
                    neighbourNode.m_hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if (!m_openList.Contains(neighbourNode)) 
                        m_openList.Add(neighbourNode);

                }
            }
        }

        // Out of nodes on the openList
        return null;
    }

    /// <summary>
    /// Returns a list of 8 neighbours around the node
    /// </summary>
    private List<PathNode> GetNeighbourList(PathNode currentNode) {
        List<PathNode> neighbourList = new List<PathNode>();

        if (currentNode.x - 1 >= 0) {
            // Add Left Node
            neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y));

            // Add left up node
            if (currentNode.y + 1 < m_grid.GetHeight()) 
                neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y + 1));

            // Add left down node
            if (currentNode.y - 1 >= 0) 
                    neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y - 1));

        }
        if (currentNode.x + 1 < m_grid.GetWidth()) {
            // Add Right Node
            neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y));

            // Add Right Down
            if (currentNode.y - 1 >= 0) 
                neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y - 1));
            // Add Right Up
            if (currentNode.y + 1 < m_grid.GetHeight()) 
                neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y + 1));
        }
        // Add Down
        if (currentNode.y - 1 >= 0) 
            neighbourList.Add(GetNode(currentNode.x, currentNode.y - 1));
        // Add Up
        if (currentNode.y + 1 < m_grid.GetHeight()) 
            neighbourList.Add(GetNode(currentNode.x, currentNode.y + 1));

        return neighbourList;
    }

    /// <summary>
    /// Get a node from the grid using x and y
    /// </summary>
    public PathNode GetNode(int x, int y) 
    {
        return m_grid.GetGridObject(x, y);
    }

    /// <summary>
    /// Once the path has been found loops through all the parent nodes of the endNode and returns a list of the path
    /// </summary>
    private List<PathNode> CalculatePath(PathNode endNode) 
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;

        while (currentNode.m_parentNode != null) 
        {
            path.Add(currentNode.m_parentNode);
            currentNode = currentNode.m_parentNode;
        }

        path.Reverse();
        return path;
    }

    /// <summary>
    /// Calculates the cost of going from node a to node b
    /// </summary>
    private int CalculateDistanceCost(PathNode a, PathNode b) 
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);

        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    /// <summary>
    /// Goes through all the nodes and finds the current lowest F cost node
    /// </summary>
    private PathNode GetLowestFcost(List<PathNode> pathList) 
    {
        PathNode lowestFCostNode = pathList[0];

        for (int i = 1; i < pathList.Count; i++) 
        {
            if (pathList[i].m_fCost < lowestFCostNode.m_fCost) 
            {
                lowestFCostNode = pathList[i];
            }
        }

        return lowestFCostNode;
    }

}
