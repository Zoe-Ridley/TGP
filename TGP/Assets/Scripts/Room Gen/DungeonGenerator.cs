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
public class Cell
{
    public bool m_visited;
    public bool[] m_generatedRoomStatus = new bool[4];
    public bool[] m_closedRoomStatus = { false, false, false, false };
    public Vector2 m_Position;
    public bool m_opened;
    public GameObject RoomObject;
    public int NumberOfEnemies;
    public List<GameObject> RangedEnemyList;
    public List<GameObject> MeleeEnemyList;
};

[Serializable]
public class DungeonGenerator : MonoBehaviour
{
    [Header("Dungeon Settings", order = 1)]
    public Vector2 RoomSize;
    public Vector2 SizeOfDungeon;
    private int m_startPosition = 0;


    [Header("Room Prefabs", order = 2)]
    [SerializeField] private GameObject FinalRoomPrefab;
    [SerializeField] private GameObject m_room;

    [Header("Enemy Prefabs & Settings", order = 3)]
    [SerializeField] private GameObject MeleeEnemy;
    [SerializeField] private int MaxMeleeEnemiesPerRoom;
    [SerializeField] private GameObject RangedEnemy;
    [SerializeField] private int MaxRangedEnemiesPerRoom;

    [Header("Generated Rooms", order = 4)]
    public Cell FinalRoom;
    public List<Cell> AllPositionsOnBoard;
    public List<Cell> GeneratedRooms;


    // Start is called before the first frame update
    void Start()
    {
        MazeGenerator();
    }

    public Cell FindRoom(Vector2 objectPosition)
    {
        objectPosition.x = Mathf.RoundToInt(objectPosition.x / RoomSize.x);
        objectPosition.y = Mathf.RoundToInt(objectPosition.y / RoomSize.y);

        for (int i = 0; i < (GeneratedRooms.Count); i++)
        {
            //checking which room the player is inside by comparing positions
            if (objectPosition == GeneratedRooms[i].m_Position)
            {
                return GeneratedRooms[i];
            }
        }
        return null;
    }    
    public void OpenRoom(Vector2 tempPlayerPosition)
    {
        tempPlayerPosition.x = Mathf.RoundToInt(tempPlayerPosition.x / RoomSize.x);
        tempPlayerPosition.y = Mathf.RoundToInt(tempPlayerPosition.y / RoomSize.y);

        for (int i = 0; i < (GeneratedRooms.Count - 1); i++)
        {
            //checking which room the player is inside by comparing positions
            if (tempPlayerPosition == GeneratedRooms[i].m_Position)
            {
                //open the door for the room currently stood inside
                GameObject playerStoodRoom = GeneratedRooms[i].RoomObject;
                playerStoodRoom.GetComponent<RoomBehaviour>().UpdateRoom(GeneratedRooms[i].m_generatedRoomStatus);

                GeneratedRooms[i].m_opened = true;

                for (int j = 0; j < 4; j++)
                {
                    var nextRoom = GeneratedRooms[i + 1];

                    //if the next room has two exits, open up the room completely.
                    var status = new[] { false, false, false, false };
                    if (GeneratedRooms[i].m_Position.y + 1 == GeneratedRooms[i + 1].m_Position.y) // ABOVE
                    {
                        status[1] = true;
                    }
                    if (GeneratedRooms[i].m_Position.y - 1 == GeneratedRooms[i + 1].m_Position.y) //BELOW
                    {
                        status[0] = true;
                    }
                    if (GeneratedRooms[i].m_Position.x - 1 == GeneratedRooms[i + 1].m_Position.x) //LEFT
                    {
                        status[2] = true;
                    }
                    if (GeneratedRooms[i].m_Position.x + 1 == GeneratedRooms[i + 1].m_Position.x) //RIGHT
                    {
                        status[3] = true;
                    }

                    nextRoom.RoomObject.GetComponent<RoomBehaviour>().UpdateRoom(status);
                }
            }
        }
    }

    void GenerateDungeon()
    {
        RoomBehaviour newRoom;
        for (int i = 0; i < SizeOfDungeon.x; i++)
        {
            for (int j = 0; j < SizeOfDungeon.y; j++)
            {
                Cell currentCell = AllPositionsOnBoard[Mathf.FloorToInt(i + j * SizeOfDungeon.x)];
                if (currentCell.m_visited)
                {
                    //Checking if currentCell is the last room of the dungeon
                    if (currentCell == AllPositionsOnBoard[AllPositionsOnBoard.Count - 1])
                    {
                        newRoom = Instantiate(FinalRoomPrefab, new Vector3(i * RoomSize.x, -j * RoomSize.y, 0f), Quaternion.identity,
                        transform).GetComponent<RoomBehaviour>();
                        FinalRoom = currentCell;
                    }
                    else
                    {
                        newRoom = Instantiate(m_room, new Vector3(i * RoomSize.x, -j * RoomSize.y, 0f), Quaternion.identity,
                        transform).GetComponent<RoomBehaviour>();

                    }

                    //Assigning the newly instantiated room object to the cell data (avoiding using Find)
                    currentCell.RoomObject = newRoom.gameObject;

                    //Generate enemies
                    GenerateRoomFeatures(currentCell);

                    /*Closes all the doors inside of the room. m_closedRoomStatus can be replaced with
                    /// m_GeneratedRoomStatus in order to generate a dungeon with doors open.*/
                    newRoom.UpdateRoom(currentCell.m_closedRoomStatus);
                    newRoom.name += " " + currentCell.m_Position.x + "-" + currentCell.m_Position.y;

                    // Setup the Pathfinding
<<<<<<< HEAD
                    Vector3 pos = new Vector3((i * RoomSize.x) - RoomSize.x / 2, (-j * RoomSize.y) - RoomSize.y / 2, 0f);
                    newRoom.AddComponent<RoomPathfindingSetup>();
                    newRoom.GetComponent<RoomPathfindingSetup>().ChangeGrid(new Grid<PathNode>(Mathf.FloorToInt(RoomSize.x), Mathf.FloorToInt(RoomSize.y), 1, pos,
=======
                    Vector3 pos = new Vector3((i * m_offset.x) - 9.5f, (-j * m_offset.y) - 9.5f, 0f);
                    newRoom.AddComponent<RoomPathfindingSetup>();
                    newRoom.GetComponent<RoomPathfindingSetup>().ChangeGrid(new Grid<PathNode>(19, 19, 1, pos, 
>>>>>>> Jaba
                        (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y)));
                }
            }
        }
    }

    public void MazeGenerator()
    {
        AllPositionsOnBoard = new List<Cell>();

        for (int j = 0; j < SizeOfDungeon.y; j++)
        {
            for (int i = 0; i < SizeOfDungeon.x; i++)
            {
                Cell tempCell = new Cell();
                tempCell.m_Position = new Vector2(i, -j);
                AllPositionsOnBoard.Add(tempCell);
            }
        }

        int m_currentCell = m_startPosition;

        Stack<int> path = new Stack<int>();

        int k = 0;

        while (k < 1000)
        {
            k++;
            AllPositionsOnBoard[m_currentCell].m_visited = true;
            GeneratedRooms.Add(AllPositionsOnBoard[m_currentCell]);

            if (m_currentCell == AllPositionsOnBoard.Count - 1)
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
                        AllPositionsOnBoard[m_currentCell].m_generatedRoomStatus[2] = true;
                        m_currentCell = newCell;
                        AllPositionsOnBoard[m_currentCell].m_generatedRoomStatus[3] = true;
                    }
                    //down
                    else
                    {
                        //open bottom door of current cell and top door of next cell
                        AllPositionsOnBoard[m_currentCell].m_generatedRoomStatus[1] = true;
                        m_currentCell = newCell;
                        AllPositionsOnBoard[m_currentCell].m_generatedRoomStatus[0] = true;
                    }
                }
                else
                {
                    //left
                    if (newCell + 1 == m_currentCell)
                    {
                        //open left door of current cell and right door of next cell
                        AllPositionsOnBoard[m_currentCell].m_generatedRoomStatus[3] = true;
                        m_currentCell = newCell;
                        AllPositionsOnBoard[m_currentCell].m_generatedRoomStatus[2] = true;
                    }
                    //top
                    else
                    {
                        //open top door of current cell and bottom door of next cell
                        AllPositionsOnBoard[m_currentCell].m_generatedRoomStatus[0] = true;
                        m_currentCell = newCell;
                        AllPositionsOnBoard[m_currentCell].m_generatedRoomStatus[1] = true;
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
        if (cell - SizeOfDungeon.x >= 0 && !AllPositionsOnBoard[Mathf.FloorToInt(cell - SizeOfDungeon.x)].m_visited)
        {
            m_neighbours.Add(Mathf.FloorToInt(cell - SizeOfDungeon.x));
        }

        //down
        if (cell + SizeOfDungeon.x < AllPositionsOnBoard.Count && !AllPositionsOnBoard[Mathf.FloorToInt(cell + SizeOfDungeon.x)].m_visited)
        {
            m_neighbours.Add(Mathf.FloorToInt(cell + SizeOfDungeon.x));
        }

        //right
        if ((cell + 1) % SizeOfDungeon.x != 0 && !AllPositionsOnBoard[Mathf.FloorToInt(cell + 1)].m_visited)
        {
            m_neighbours.Add(Mathf.FloorToInt(cell + 1));
        }

        //left
        if (cell % SizeOfDungeon.x != 0 && !AllPositionsOnBoard[Mathf.FloorToInt(cell - 1)].m_visited)
        {
            m_neighbours.Add(Mathf.FloorToInt(cell - 1));
        }

        return m_neighbours;
    }

    public void GenerateRoomFeatures(Cell currentCell)
    {
        Vector2 roomSize = currentCell.RoomObject.GetComponent<RoomBehaviour>().RoomSize;
        Vector2 roomPosition = currentCell.RoomObject.transform.position;
        Vector2 minPosition = new Vector2((roomPosition.x - roomSize.x / 2), roomPosition.y - roomSize.y / 2);
        Vector2 maxPosition = new Vector2((roomPosition.x + roomSize.x / 2), roomPosition.y + roomSize.y / 2);

        //Generate enemy count
        int rangedEnemyCount = Random.Range(1, MaxRangedEnemiesPerRoom);
        int meleeEnemyCount = Random.Range(1, MaxMeleeEnemiesPerRoom);

        for (int i = 0; i < rangedEnemyCount; i++)
        {
            GameObject newRangedEnemy = Instantiate(RangedEnemy, GenerateRandomPosition(minPosition, maxPosition), Quaternion.identity);
            currentCell.NumberOfEnemies++;
        }

        for (int i = 0; i < meleeEnemyCount; i++)
        {
            //Melee enemies rely on having the Room object as their parent in order to configure pathfinding.
            GameObject newMeleeEnemy = Instantiate(MeleeEnemy, currentCell.RoomObject.transform);
            newMeleeEnemy.transform.position = GenerateRandomPosition(minPosition, maxPosition);
            currentCell.NumberOfEnemies++;
        }
    }

    private Vector2 GenerateRandomPosition(Vector2 min, Vector2 max)
    {
        var x = Random.Range(min.x, max.x);
        var y = Random.Range(min.y, max.y);
        return new Vector2(x, y);
    }
}