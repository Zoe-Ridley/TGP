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
    public int NumberOfEnemies;
    public bool Visited;
    public bool Opened;
    public bool[] GeneratedRoomStatus = new bool[4];
    public bool[] ClosedRoomStatus = { false, false, false, false };
    public Vector2 Position;
    public GameObject RoomObject;
    public List<GameObject> RoomObstacles;
    public List<GameObject> RoomEnemies;
};

[Serializable]
public class DungeonGenerator : MonoBehaviour
{
    [Header("Dungeon Settings", order = 1)]
    public Vector2 RoomSize;
    public Vector2 SizeOfDungeon;
    private int m_startPosition = 0;

    [Header("Room Prefabs", order = 2)]
    [SerializeField] private GameObject m_finalRoomPrefab;
    [SerializeField] private GameObject m_room;

    [Header("Enemy Prefabs & Settings", order = 3)]
    [SerializeField] private GameObject m_meleeEnemy;
    [SerializeField] private int m_maxMeleeEnemiesPerRoom;
    [SerializeField] private GameObject m_rangedEnemy;
    [SerializeField] private int m_maxRangedEnemiesPerRoom;

    [Header("Obstacles", order = 4)] 
    public GameObject ObstaclePrefab;
    [SerializeField] private int m_maxObstaclesPerRoom;

    [Header("Generated Rooms", order = 5)]
    public Vector2 CentreOfDungeon;
    public Cell FinalRoom;
    public List<Cell> AllPositionsOnBoard;
    public List<Cell> GeneratedRooms;

    [Header("Additional Objects", order = 6)] 
    public GameObject MinimapCamera;


    //Start is called before the first frame update
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
            //Checking which room the player is inside by comparing positions
            if (objectPosition == GeneratedRooms[i].Position)
            {
                return GeneratedRooms[i];
            }
        }
        return null;
    }    

    public void OpenRoom(Vector2 tempPlayerPosition)
    {
        Cell cellToFind = FindRoom(tempPlayerPosition);


        for (int i = 0; i < (GeneratedRooms.Count - 1); i++)
        {
            //Checking which room the player is inside by comparing positions
            if (cellToFind == GeneratedRooms[i])
            {
                //Open the door for the room currently stood inside
                GameObject playerStoodRoom = GeneratedRooms[i].RoomObject;
                playerStoodRoom.GetComponent<RoomBehaviour>().UpdateRoom(GeneratedRooms[i].GeneratedRoomStatus);

                GeneratedRooms[i].Opened = true;

                foreach (var enemy in GeneratedRooms[i+1].RoomEnemies)
                {
                    enemy.SetActive(true);
                }

                for (int j = 0; j < 4; j++)
                {
                    var nextRoom = GeneratedRooms[i + 1];

                    //If the next room has two exits, open up the room completely.
                    var status = new[] { false, false, false, false };
                    if (GeneratedRooms[i].Position.y + 1 == GeneratedRooms[i + 1].Position.y) //Above
                    {
                        status[1] = true;
                    }
                    if (GeneratedRooms[i].Position.y - 1 == GeneratedRooms[i + 1].Position.y) //Below
                    {
                        status[0] = true;
                    }
                    if (GeneratedRooms[i].Position.x - 1 == GeneratedRooms[i + 1].Position.x) //Left
                    {
                        status[2] = true;
                    }
                    if (GeneratedRooms[i].Position.x + 1 == GeneratedRooms[i + 1].Position.x) //Right
                    {
                        status[3] = true;
                    }

                    nextRoom.RoomObject.GetComponent<RoomBehaviour>().UpdateRoom(status);
                    nextRoom.RoomObject.GetComponent<RoomBehaviour>().EnableLighting();
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
                   
                if (currentCell.Visited)
                {
                    //Checking if currentCell is the last room of the dungeon
                    if (currentCell == AllPositionsOnBoard[AllPositionsOnBoard.Count - 1])
                    {
                        newRoom = Instantiate(m_finalRoomPrefab, new Vector3(i * RoomSize.x, -j * RoomSize.y, .9f), Quaternion.identity,
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

                    /*Closes all the doors inside of the room. ClosedRoomStatus can be replaced with
                    m_GeneratedRoomStatus in order to generate a dungeon with doors open.*/
                    newRoom.UpdateRoom(currentCell.ClosedRoomStatus);
                    newRoom.name += " " + currentCell.Position.x + "-" + currentCell.Position.y;

                    //Setup the Pathfinding
                    Vector3 pos = new Vector3((i * RoomSize.x) - RoomSize.x / 2, (-j * RoomSize.y) - RoomSize.y / 2, 0f);
                    newRoom.AddComponent<RoomPathfindingSetup>();
                    newRoom.GetComponent<RoomPathfindingSetup>().ChangeGrid(new Grid<PathNode>(Mathf.FloorToInt(RoomSize.x), Mathf.FloorToInt(RoomSize.y), 1, pos,
                        (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y)));
                }
            }
        }
        //Turn on the lighting for the first room and enable enemies
        GeneratedRooms[0].RoomObject.GetComponent<RoomBehaviour>().EnableLighting();
        foreach (var enemy in GeneratedRooms[0].RoomEnemies)
        {
            enemy.SetActive(true);
        }

        //Find the middle of the dungeon
        Vector2 calculatedDungeonSize = new Vector2(((RoomSize.x * SizeOfDungeon.x) / 2) - (RoomSize.x/2) , ((RoomSize.y * -SizeOfDungeon.y) /2) + (RoomSize.y / 2));
        
        CentreOfDungeon = calculatedDungeonSize;
        MinimapCamera.transform.position = calculatedDungeonSize;
    }

    public void MazeGenerator()
    {
        AllPositionsOnBoard = new List<Cell>();

        for (int j = 0; j < SizeOfDungeon.y; j++)
        {
            for (int i = 0; i < SizeOfDungeon.x; i++)
            {
                Cell tempCell = new Cell();
                tempCell.Position = new Vector2(i, -j);
                AllPositionsOnBoard.Add(tempCell);
            }
        }

        int m_currentCell = m_startPosition;

        Stack<int> path = new Stack<int>();

        int k = 0;

        while (k < 1000)
        {
            k++;
            AllPositionsOnBoard[m_currentCell].Visited = true;
            GeneratedRooms.Add(AllPositionsOnBoard[m_currentCell]);

            if (m_currentCell == AllPositionsOnBoard.Count - 1)
            {
                break;
            }

            //Check the cells neighbours
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

                //If path is going down or right
                if (newCell > m_currentCell)
                {
                    //Right
                    if (newCell - 1 == m_currentCell)
                    {
                        //Open right door of current cell and left door of next cell
                        AllPositionsOnBoard[m_currentCell].GeneratedRoomStatus[2] = true;
                        m_currentCell = newCell;
                        AllPositionsOnBoard[m_currentCell].GeneratedRoomStatus[3] = true;
                    }
                    //Down
                    else
                    {
                        //Open bottom door of current cell and top door of next cell
                        AllPositionsOnBoard[m_currentCell].GeneratedRoomStatus[1] = true;
                        m_currentCell = newCell;
                        AllPositionsOnBoard[m_currentCell].GeneratedRoomStatus[0] = true;
                    }
                }
                else
                {
                    //Left
                    if (newCell + 1 == m_currentCell)
                    {
                        //Open left door of current cell and right door of next cell
                        AllPositionsOnBoard[m_currentCell].GeneratedRoomStatus[3] = true;
                        m_currentCell = newCell;
                        AllPositionsOnBoard[m_currentCell].GeneratedRoomStatus[2] = true;
                    }
                    //Top
                    else
                    {
                        //Open top door of current cell and bottom door of next cell
                        AllPositionsOnBoard[m_currentCell].GeneratedRoomStatus[0] = true;
                        m_currentCell = newCell;
                        AllPositionsOnBoard[m_currentCell].GeneratedRoomStatus[1] = true;
                    }
                }
            }
        }
        GenerateDungeon();
    }

    public List<int> CheckNeighbours(int cell)
    {
        List<int> m_neighbours = new List<int>();

        //Up
        if (cell - SizeOfDungeon.x >= 0 && !AllPositionsOnBoard[Mathf.FloorToInt(cell - SizeOfDungeon.x)].Visited)
        {
            m_neighbours.Add(Mathf.FloorToInt(cell - SizeOfDungeon.x));
        }

        //Down
        if (cell + SizeOfDungeon.x < AllPositionsOnBoard.Count && !AllPositionsOnBoard[Mathf.FloorToInt(cell + SizeOfDungeon.x)].Visited)
        {
            m_neighbours.Add(Mathf.FloorToInt(cell + SizeOfDungeon.x));
        }

        //Right
        if ((cell + 1) % SizeOfDungeon.x != 0 && !AllPositionsOnBoard[Mathf.FloorToInt(cell + 1)].Visited)
        {
            m_neighbours.Add(Mathf.FloorToInt(cell + 1));
        }

        //Left
        if (cell % SizeOfDungeon.x != 0 && !AllPositionsOnBoard[Mathf.FloorToInt(cell - 1)].Visited)
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

        currentCell.RoomEnemies = new List<GameObject>();

        //Generate enemy and obstacle count
        int rangedEnemyCount = Random.Range(1, m_maxRangedEnemiesPerRoom);
        int meleeEnemyCount = Random.Range(1, m_maxMeleeEnemiesPerRoom);
        int obstacleCount = Random.Range(1, m_maxObstaclesPerRoom);

        //Instantiate enemies
        for (int i = 0; i < rangedEnemyCount; i++)
        {
            GameObject newRangedEnemy = Instantiate(m_rangedEnemy, currentCell.RoomObject.transform);
            newRangedEnemy.transform.position = GenerateRandomPosition(minPosition, maxPosition);
            currentCell.NumberOfEnemies++;
            currentCell.RoomEnemies.Add(newRangedEnemy);
            newRangedEnemy.SetActive(false);
        }

        for (int i = 0; i < meleeEnemyCount; i++)
        {
            //Melee enemies rely on having the Room object as their parent in order to configure pathfinding.
            GameObject newMeleeEnemy = Instantiate(m_meleeEnemy, currentCell.RoomObject.transform);
            newMeleeEnemy.transform.position = GenerateRandomPosition(minPosition, maxPosition);

            

            currentCell.NumberOfEnemies++;
            currentCell.RoomEnemies.Add(newMeleeEnemy);
            newMeleeEnemy.SetActive(false);
        }

        //Create obstacles
        for (int i = 0; i < obstacleCount; i++)
        {
            GameObject newObstacle = Instantiate(ObstaclePrefab, currentCell.RoomObject.transform);
            newObstacle.transform.position = GenerateRandomPosition(minPosition, maxPosition);
            //if(newObstacle.GetComponent<BoxCollider2D>().IsTouching())
            currentCell.RoomObstacles.Add(newObstacle);
        }
    }

    private Vector2 GenerateRandomPosition(Vector2 min, Vector2 max)
    {
        var x = Mathf.FloorToInt(Random.Range(min.x, max.x));
        var y = Mathf.FloorToInt(Random.Range(min.y, max.y));
        return new Vector2(x, y);
    }
}