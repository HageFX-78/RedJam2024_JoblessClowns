using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomFactory : MonoBehaviour
{
    [Header("Room Configs")]
    [SerializeField] GameObject lobbyRoomPrefab;
    [SerializeField] GameObject tappyRoomPrefab;
    [SerializeField] GameObject bamRoomPrefab;
    [SerializeField] GameObject oguRoomPrefab;

    public GameObject PlaceRoom(RoomType roomType, int roomsX, int roomsY)
    {
        Vector3 roomPosition = new();

        switch (roomType)
        {
            case RoomType.Lobby:
                return Instantiate(lobbyRoomPrefab, roomPosition, Quaternion.identity);

            case RoomType.Tappy:
                return Instantiate(tappyRoomPrefab, roomPosition, Quaternion.identity);

            case RoomType.Bam:
                return Instantiate(bamRoomPrefab, roomPosition, Quaternion.identity);

            case RoomType.Ogu:
                return Instantiate(oguRoomPrefab, roomPosition, Quaternion.identity);
        }

        //fallback, this should not happen
        Debug.LogError("Failed to create room, check room enum passed in");
        return null;
    }
}
