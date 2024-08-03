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

    [Header("Map Components")]
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase floorTile;

    [Header("Room Configs")]
    [SerializeField] GameObject lobbyRoomPrefab;
    [SerializeField] GameObject tappyRoomPrefab;
    [SerializeField] GameObject bamRoomPrefab;
    [SerializeField] GameObject oguRoomPrefab;

    [Header("Map Config")]
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

        for (int x = 0; x < mapLength; x++)
        {
            for(int y= 0; y < mapWidth; y++)
            {
                var tileLocation = new Vector3Int(x - xOffset, y - yOffset, 0);

                tilemap.SetTile(tileLocation, floorTile);
            }
        }
    }

    private void GenerateRooms()
    {
        int roomsX = mapWidth / roomSize;
        int roomsY = mapLength / roomSize;

        bool[,] rooms = new bool[roomsX, roomsY];

        //set start room
        Vector3Int startRoom = GetEdgeRoom(roomsX, roomsY);
        rooms[startRoom.x, startRoom.y] = true;

        //PlaceRoom(RoomType.Lobby, startRoom.x, startRoom.y);

        foreach(RoomType type in Enum.GetValues(typeof(RoomType)))
        {
            if(type == RoomType.Lobby)
            {
                continue;
            }

            //PlaceRoom(type, );
        }



#if UNITY_EDITOR
        Debug.Log($"Location of lobby room is {startRoom}");
#endif
    }

    private void PlaceRoom(RoomType roomType, int roomsX, int roomsY)
    {
        Vector3 roomPosition = new();

        switch (roomType)
        {
            case RoomType.Lobby:
                Instantiate(lobbyRoomPrefab, roomPosition, Quaternion.identity);
                break;

            case RoomType.Tappy:
                Instantiate(tappyRoomPrefab, roomPosition, Quaternion.identity);

                break;

            case RoomType.Bam:
                Instantiate(bamRoomPrefab, roomPosition, Quaternion.identity);

                break;

            case RoomType.Ogu:
                Instantiate(oguRoomPrefab, roomPosition, Quaternion.identity);
                break;
        }

    }

    private Vector2Int GetRandomRoom()
    {
        return new();
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
