using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid<TGridObject> {

    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
    public class OnGridObjectChangedEventArgs : EventArgs 
    {
        public int x;
        public int y;
    }

    private int m_width;
    private int m_height;
    private float m_tileSize;
    private Vector3 m_originPos;
    private TGridObject[,] m_gridArray;

    public Grid(int width, int height, float tileSize, Vector3 originPosition, 
        Func<Grid<TGridObject>, int, int, TGridObject> CreateGridObject) 
    {
        m_width = width;
        m_height = height;
        m_tileSize = tileSize;
        m_originPos = originPosition;

        m_gridArray = new TGridObject[width, height];

        for (int x = 0; x < m_gridArray.GetLength(0); x++) {
            for (int y = 0; y < m_gridArray.GetLength(1); y++) {
                m_gridArray[x, y] = CreateGridObject(this, x, y);
            }
        }

        // set debug to true to draw the grid 
        bool showDebug = true;
        if (showDebug) 
        {

            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {       
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                }
            }

            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
        }
    }

    /// <summary>
    /// returns the width of the grid
    /// </summary>
    public int GetWidth()
    { 
        return m_width;
    }

    /// <summary>
    /// returns the height of the grid
    /// </summary>
    public int GetHeight() 
    {
        return m_height;
    }

    /// <summary>
    /// returns the size of the grid
    /// </summary>
    public float GetCellSize() 
    {
        return m_tileSize;
    }

    /// <summary>
    /// Converts x and y location of the grid to a world position
    /// </summary>
    public Vector3 GetWorldPosition(int x, int y) 
    {
        return new Vector3(x, y) * m_tileSize + m_originPos;
    }

    /// <summary>
    /// <para>Returns the x and y of the world position on the grid </para>
    /// <para>The function outputs the result to the passed in x and y</para>
    /// </summary>
    public void GetXY(Vector3 worldPos, out int x, out int y) 
    {
        x = Mathf.FloorToInt((worldPos - m_originPos).x / m_tileSize);
        y = Mathf.FloorToInt((worldPos - m_originPos).y / m_tileSize);
    }

    public void SetGridObject(int x, int y, TGridObject value) 
    {
        if (x >= 0 && y >= 0 && x < m_width && y < m_height) 
        {
            m_gridArray[x, y] = value;

            if (OnGridObjectChanged != null) 
                OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y });
        }
    }

    public void TriggerGridObjectChanged(int x, int y) 
    {
        if (OnGridObjectChanged != null) 
            OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y });
    }

    public void SetGridObject(Vector3 worldPosition, TGridObject value) 
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetGridObject(x, y, value);
    }

    public TGridObject GetGridObject(int x, int y) 
    {
        if (x >= 0 && y >= 0 && x < m_width && y < m_height) 
        {
            return m_gridArray[x, y];
        } 
        else 
        {
            Debug.Log("x or why out of bounds - " + this.ToString());
            return default(TGridObject);
        }
    }

    public TGridObject GetGridObject(Vector3 worldPosition) 
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetGridObject(x, y);
    }

}
