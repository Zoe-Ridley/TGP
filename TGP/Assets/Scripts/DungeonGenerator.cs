using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DungeonGenerator : MonoBehaviour
{
    public class Cell
    {
        public bool m_visited = false;
        public bool[] m_status = new bool[4];
    }

    public Vector2 m_size;
    public int m_startPosition = 0;
    public GameObject m_room;
    public Vector2 m_offset;

    private List<Cell> m_board;

    // Start is called before the first frame update
    void Start()
    {
        MazeGenerator();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void GenerateDungeon()
    {
        for (int i = 0; i < m_size.x; i++)
        {
            for (int j = 0; j < m_size.y; j++)
            {
                Cell currentCell = m_board[Mathf.FloorToInt(i + j * m_size.x)];
                if (currentCell.m_visited)
                {
                    var newRoom = Instantiate(m_room, new Vector3(i * m_offset.x, -j * m_offset.y, 0f), Quaternion.identity, transform).GetComponent<RoomBehaviour>();
                    newRoom.UpdateRoom(currentCell.m_status);

                    newRoom.name += " " + i + "-" + j;
                }
            }
        }
    }

    public void MazeGenerator()
    {
        m_board = new List<Cell>();

        for (int i = 0; i < m_size.x; i++)
        {
            for (int j = 0; j < m_size.y; j++)
            {
                m_board.Add(new Cell());
            }
        }

        int m_currentCell = m_startPosition;

        Stack<int> path = new Stack<int>();

        int k = 0;

        while (k < 1000)
        {
            k++;
            m_board[m_currentCell].m_visited = true;

            if (m_currentCell == m_board.Count - 1)
            {
                break;
            }

            //check the cell's neighbours
            List<int> neighbours = CheckNeighbours(m_currentCell);

            if (neighbours.Count == 0)
            {
                if (path.Count == 0)
                {
                    break;
                }
                else
                {
                    m_currentCell = path.Pop();
                }
            }
            else
            {
                path.Push(m_currentCell);

                int newCell = neighbours[Random.Range(0, neighbours.Count)];

                //if path is going down or right
                if (newCell > m_currentCell)
                {
                    //right
                    if (newCell - 1 == m_currentCell)
                    {
                        //open right door of current cell and left door of next cell
                        m_board[m_currentCell].m_status[2] = true;
                        m_currentCell = newCell;
                        m_board[m_currentCell].m_status[3] = true;
                    }
                    //down
                    else
                    {
                        //open bottom door of current cell and top door of next cell
                        m_board[m_currentCell].m_status[1] = true;
                        m_currentCell = newCell;
                        m_board[m_currentCell].m_status[0] = true;
                    }
                }
                else
                {
                    //left
                    if (newCell + 1 == m_currentCell)
                    {
                        //open left door of current cell and right door of next cell
                        m_board[m_currentCell].m_status[3] = true;
                        m_currentCell = newCell;
                        m_board[m_currentCell].m_status[2] = true;
                    }
                    //top
                    else
                    {
                        //open top door of current cell and bottom door of next cell
                        m_board[m_currentCell].m_status[0] = true;
                        m_currentCell = newCell;
                        m_board[m_currentCell].m_status[1] = true;
                    }
                }
            }
        }
        GenerateDungeon();
    }

    public List<int> CheckNeighbours(int cell)
    {
        List<int> m_neighbours = new List<int>();

        //up neighbour
        if (cell - m_size.x >= 0 && !m_board[Mathf.FloorToInt(cell - m_size.x)].m_visited)
        {
            m_neighbours.Add(Mathf.FloorToInt(cell - m_size.x));
        }

        //down neighbour
        if (cell + m_size.x < m_board.Count && !m_board[Mathf.FloorToInt(cell + m_size.x)].m_visited)
        {
            m_neighbours.Add(Mathf.FloorToInt(cell + m_size.x));
        }

        //right neighbour
        if ((cell + 1) % m_size.x != 0 && !m_board[Mathf.FloorToInt(cell + 1)].m_visited)
        {
            m_neighbours.Add(Mathf.FloorToInt(cell + 1));
        }

        //left neighbour
        if (cell % m_size.x != 0 && !m_board[Mathf.FloorToInt(cell - 1)].m_visited)
        {
            m_neighbours.Add(Mathf.FloorToInt(cell - 1));
        }

        return m_neighbours;
    }
}
