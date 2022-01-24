using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using UnityEngine;
using Random = UnityEngine.Random;

public class DungeonGenerator : MonoBehaviour
{
    public class Cell
    {
        public bool m_visited = false;
        public bool[] m_generatedRoomStatus = new bool[4];
        public bool[] m_closedRoomStatus = {false, false, false, false};
        public Vector2 m_Position;
    }

    public struct Room
    {
        public Cell m_cell;
        public int m_boardPosition;
    };

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

    public void OpenRoom(GameObject searchRoom)
    {
        Vector2 desiredLocation = new Vector2(Mathf.FloorToInt(searchRoom.transform.position.x), Mathf.FloorToInt(searchRoom.transform.position.y));
        for (int i = 0; i < m_board.Count; i++)
        {
            if (desiredLocation == m_board[i].m_Position)
            {
                searchRoom.GetComponent<RoomBehaviour>().UpdateRoom(m_board[i].m_generatedRoomStatus);
                for (int j = 0; j < 4; j++)
                {
                    if (m_board[i].m_Position.x + 1 == m_board[i + 1].m_Position.x)
                    {
                        //next cell in the path is to the right
                    }
                    else if (m_board[i].m_Position.x - 1 == m_board[i + 1].m_Position.x)
                    {
                        //next cell in the path is to the left
                    }
                    else if (m_board[i].m_Position.y + 1 == m_board[i + 1].m_Position.y)
                    {
                        //next cell in the path is above
                    }
                    else if (m_board[i].m_Position.y - 1 == m_board[i - 1].m_Position.y)
                    {
                        //next call is below
                    } 

                }
                break;
            }
        }
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
                    //newRoom.UpdateRoom(currentCell.m_generatedRoomStatus);
                    newRoom.UpdateRoom(currentCell.m_closedRoomStatus);

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
                Cell tempCell = new Cell();
                tempCell.m_Position = new Vector2(i, j);
                m_board.Add(tempCell);
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
                        m_board[m_currentCell].m_generatedRoomStatus[2] = true;
                        m_currentCell = newCell;
                        m_board[m_currentCell].m_generatedRoomStatus[3] = true;
                    }
                    //down
                    else
                    {
                        //open bottom door of current cell and top door of next cell
                        m_board[m_currentCell].m_generatedRoomStatus[1] = true;
                        m_currentCell = newCell;
                        m_board[m_currentCell].m_generatedRoomStatus[0] = true;
                    }
                }
                else
                {
                    //left
                    if (newCell + 1 == m_currentCell)
                    {
                        //open left door of current cell and right door of next cell
                        m_board[m_currentCell].m_generatedRoomStatus[3] = true;
                        m_currentCell = newCell;
                        m_board[m_currentCell].m_generatedRoomStatus[2] = true;
                    }
                    //top
                    else
                    {
                        //open top door of current cell and bottom door of next cell
                        m_board[m_currentCell].m_generatedRoomStatus[0] = true;
                        m_currentCell = newCell;
                        m_board[m_currentCell].m_generatedRoomStatus[1] = true;
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
