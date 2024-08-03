using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private GameObject grid;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase floorTile;

    [Header("Map Config - unit in tiles")]
    [SerializeField] private int mapLength;
    [SerializeField] private int mapWidth;
    [SerializeField] private int roomSize = 1;

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

        int maxIndex = mapLength * mapWidth;

        for (int i = 0; i < maxIndex; i++)
        {
            int row = i / mapWidth; // determines row
            int column = i % mapWidth; // determines column

            var tileLocation = new Vector3Int(row , column , 0);
            tilemap.SetTile(tileLocation, floorTile);
        }
    }

    private void GenerateRooms()
    {
        int roomsX = mapWidth / roomSize;
        int roomsY = mapLength / roomSize;

        bool[,] rooms = new bool[roomsX, roomsY];

        Vector3 convertedWorldPosition = new();
        Vector3Int roomLocation;

        foreach(RoomType type in Enum.GetValues(typeof(RoomType)))
        {
            if(type == RoomType.Lobby)
            {
                roomLocation = GetEdgeRoom(roomsX, roomsY);
            }
            else
            {
                roomLocation = GetRandomRoom(rooms, roomsX, roomsY);
            }

            //mark it as occupied
            rooms[roomLocation.x, roomLocation.y] = true;

            //create rooms and set its position
            GameObject roomPrefab = roomFactory.PlaceRoom(grid.transform, type);

            convertedWorldPosition = ConvertRoomsToWorldPosition(roomLocation, roomPrefab);
            roomPrefab.transform.position = convertedWorldPosition;

#if UNITY_EDITOR
            Debug.Log($"{type}'s room location is {roomLocation}");
            Debug.Log($"{type}'s world location is {convertedWorldPosition}");
#endif
        }
    }

    private Vector3 ConvertRoomsToWorldPosition(Vector3Int roomLocation, GameObject roomPrefab)
    {
        Tilemap roomTilemap = roomPrefab.GetComponent<Tilemap>();
        roomTilemap.CompressBounds();

        var newLocation = new Vector3((roomLocation.x * roomSize), (roomLocation.y * roomSize), 0);

        return newLocation;
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
