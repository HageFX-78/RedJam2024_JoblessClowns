using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomFactory : MonoBehaviour
{
    [Header("Room Configs")]
    [SerializeField] GameObject lobbyRoomPrefab;
    [SerializeField] GameObject tappyRoomPrefab;
    [SerializeField] GameObject bamRoomPrefab;
    [SerializeField] GameObject oguRoomPrefab;

    public GameObject PlaceRoom(Transform gridParent, RoomType roomType)
    {
        GameObject room = null;

        switch (roomType)
        {
            case RoomType.Lobby:
                room = Instantiate(lobbyRoomPrefab, gridParent);
                break;

            case RoomType.Tappy:
                room = Instantiate(tappyRoomPrefab, gridParent);
                break;

            case RoomType.Bam:
                room = Instantiate(bamRoomPrefab, gridParent);
                break;

            case RoomType.Ogu:
                room = Instantiate(oguRoomPrefab, gridParent);
                break;
        }

        return room;
    }
}
