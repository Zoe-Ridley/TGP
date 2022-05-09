using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

//TODO: Make a variable for width and height of the rooms generated


[Serializable]
public class DungeonGenerator : MonoBehaviour
{
    [Serializable]
    public class Cell
    {
        public bool m_visited = false;
        public bool[] m_generatedRoomStatus = new bool[4];
        public bool[] m_closedRoomStatus = { false, false, false, false };
        public Vector2 m_Position;
        public bool m_opened;
    };

    public Vector2 m_size;
    public int m_startPosition = 0;
    public GameObject m_room;
    public Vector2 m_offset;

    public List<Cell> m_board;
    public List<Cell> m_GeneratedRooms;

    // Start is called before the first frame update
    void Start()
    {
        MazeGenerator();
    }

    public void OpenRoom(Vector2 tempPlayerPosition)
    {
        //search room is the room prefab instance that the player is currently stood inside
        //find its int position and compare it to all tiles in the board

        tempPlayerPosition.x = Mathf.RoundToInt(tempPlayerPosition.x / m_offset.x);
        tempPlayerPosition.y = Mathf.RoundToInt(tempPlayerPosition.y / m_offset.y);

        for (int i = 0; i < (m_GeneratedRooms.Count - 1); i++)
        {
            if (tempPlayerPosition == m_GeneratedRooms[i].m_Position)
            {
                //open the door for the room currently stood inside
                GameObject playerStoodRoom = GameObject.Find("Room(Clone) " + (m_GeneratedRooms[i].m_Position.x) + "-" + (m_GeneratedRooms[i].m_Position.y));
                playerStoodRoom.GetComponent<RoomBehaviour>().UpdateRoom(m_GeneratedRooms[i].m_generatedRoomStatus);

                m_GeneratedRooms[i].m_opened = true;

                for (int j = 0; j < 4; j++)
                {
                    GameObject nextRoom = GameObject.Find("Room(Clone) " + (m_GeneratedRooms[i + 1].m_Position.x) + "-" + (m_GeneratedRooms[i + 1].m_Position.y));

                    //if the next room has two exits, open up the room completely for now.
                    var status = new[] { false, false, false, false };
                    if (m_GeneratedRooms[i].m_Position.y + 1 == m_GeneratedRooms[i + 1].m_Position.y) // ABOVE
                    {
                        status[1] = true;
                    }
                    if (m_GeneratedRooms[i].m_Position.y - 1 == m_GeneratedRooms[i + 1].m_Position.y) //BELOW
                    {
                        status[0] = true;
                    }
                    if (m_GeneratedRooms[i].m_Position.x - 1 == m_GeneratedRooms[i + 1].m_Position.x) //LEFT
                    {
                        status[2] = true;
                    }
                    if (m_GeneratedRooms[i].m_Position.x + 1 == m_GeneratedRooms[i + 1].m_Position.x) //RIGHT
                    {
                        status[3] = true;
                    }

                    nextRoom.GetComponent<RoomBehaviour>().UpdateRoom(status);
                }
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
                    var newRoom = Instantiate(m_room, new Vector3(i * m_offset.x, -j * m_offset.y, 0f), Quaternion.identity,
                            transform).GetComponent<RoomBehaviour>();
                    //newRoom.UpdateRoom(currentCell.m_generatedRoomStatus);
                    newRoom.UpdateRoom(currentCell.m_closedRoomStatus);
                    newRoom.name += " " + currentCell.m_Position.x + "-" + currentCell.m_Position.y;

                    // Setup the Pathfinding
                    Vector3 pos = new Vector3((i * m_offset.x) - 5.5f, (-j * m_offset.y) - 5.5f, 0f);
                    newRoom.AddComponent<RoomPathfindingSetup>();
                    newRoom.GetComponent<RoomPathfindingSetup>().ChangeGrid(new Grid<PathNode>(11, 11, 1, pos, 
                        (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y)));
                }
            }
        }
    }

    public void MazeGenerator()
    {
        m_board = new List<Cell>();

        for (int j = 0; j < m_size.y; j++)
        {
            for (int i = 0; i < m_size.x; i++)
            {
                Cell tempCell = new Cell();
                tempCell.m_Position = new Vector2(i, -j);
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
            m_GeneratedRooms.Add(m_board[m_currentCell]);

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

        //up
        if (cell - m_size.x >= 0 && !m_board[Mathf.FloorToInt(cell - m_size.x)].m_visited)
        {
            m_neighbours.Add(Mathf.FloorToInt(cell - m_size.x));
        }

        //down
        if (cell + m_size.x < m_board.Count && !m_board[Mathf.FloorToInt(cell + m_size.x)].m_visited)
        {
            m_neighbours.Add(Mathf.FloorToInt(cell + m_size.x));
        }

        //right
        if ((cell + 1) % m_size.x != 0 && !m_board[Mathf.FloorToInt(cell + 1)].m_visited)
        {
            m_neighbours.Add(Mathf.FloorToInt(cell + 1));
        }

        //left
        if (cell % m_size.x != 0 && !m_board[Mathf.FloorToInt(cell - 1)].m_visited)
        {
            m_neighbours.Add(Mathf.FloorToInt(cell - 1));
        }

        return m_neighbours;
    }
}