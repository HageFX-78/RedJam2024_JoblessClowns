using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public enum RoomType
{
    Lobby,
    Tappy,
    Bam,
    Ogu,
}

public class MapGeneration : MonoBehaviour
{
    public static MapGeneration Instance;

    [SerializeField] private RoomFactory roomFactory;

    [Header("Map Components")]
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase floorTile;

    [Header("Map Config - unit in tiles")]
    [SerializeField] private int mapLength;
    [SerializeField] private int mapWidth;
    [SerializeField] private int roomSize = 1;


    private int[,] tilemapArray;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        RenderMap();
        GenerateRooms();
    }

    #region Internal methods
    private void RenderMap()
    {
        tilemap.ClearAllTiles();

        var xOffset = Mathf.RoundToInt(mapLength / 2);
        var yOffset = Mathf.RoundToInt(mapWidth / 2);

        int maxIndex = mapLength * mapWidth;

        for (int i = 0; i < maxIndex; i++)
        {
            int row = i / mapWidth; // determines row
            int column = i % mapWidth; // determines column

            var tileLocation = new Vector3Int(row - xOffset, column - yOffset, 0);
            tilemap.SetTile(tileLocation, floorTile);
        }
    }

    private void GenerateRooms()
    {
        int roomsX = mapWidth / roomSize;
        int roomsY = mapLength / roomSize;

        bool[,] rooms = new bool[roomsX, roomsY];


        foreach(RoomType type in Enum.GetValues(typeof(RoomType)))
        {
            if(type == RoomType.Lobby)
            {
                //set start room
                Vector3Int startRoom = GetEdgeRoom(roomsX, roomsY);
                rooms[startRoom.x, startRoom.y] = true;

                //GameObject room = roomFactory.PlaceRoom(RoomType.Lobby, startRoom.x, startRoom.y);

#if UNITY_EDITOR
                Debug.Log($"Location of lobby room is {startRoom}");
#endif

                continue;
            }


            Vector3Int roomLocation = GetRandomRoom(rooms, roomsX, roomsY);
            rooms[roomLocation.x, roomLocation.y] = true;

            //GameObject room = roomFactory.PlaceRoom(type, roomLocation.x, roomLocation.y);

#if UNITY_EDITOR
            Debug.Log($"{type}'s location is {roomLocation}");
#endif
        }
    }

    private Vector3Int GetRandomRoom(bool[,] rooms, int roomsX, int roomsY)
    {
        int row, column;

        do
        {
            row = Random.Range(0, roomsX);
            column = Random.Range(0, roomsY);
        } 
        while (rooms[row, column]);

        return new Vector3Int(row, column, 0);
    }

    private Vector3Int GetEdgeRoom(int roomsX, int roomsY)
    {
        List<Vector3Int> edgeRooms = new();

        //get top & btm edge
        for (int x = 0; x < roomsX; x++)
        {
            edgeRooms.Add(new Vector3Int(x, 0));
            edgeRooms.Add(new Vector3Int(x, roomsY - 1));
        }

        //get left & right edge
        for (int y = 0; y < roomsY; y++)
        {
            edgeRooms.Add(new Vector3Int(0, y));
            edgeRooms.Add(new Vector3Int(roomsX - 1, y));
        }

        int randomRoom = Random.Range(0, edgeRooms.Count);
        return edgeRooms[randomRoom];
    }
    #endregion
}
