using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGeneration : MonoBehaviour
{
    public static MapGeneration Instance;

    [Header("Map Components")]
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase floorTile;
    [SerializeField] List<GameObject> roomPrefabs;

    [Header("Map Config")]
    [SerializeField] private int mapLength;
    [SerializeField] private int mapWidth;
    [SerializeField] private int numberOfRooms;

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

    private void GenerateMap()
    {

    }
}
