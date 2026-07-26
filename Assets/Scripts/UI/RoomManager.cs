using UnityEngine;
using System.Collections.Generic;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform camera;
    [SerializeField] private LevelBuilder levelBuilder;

    private Dictionary<Vector2Int, Room> _rooms;
    private Vector2Int _currentRoom;

    private void Start()
    {
        LoadFloor();
    }

    private void Update()
    {
        var room = RoomAt(player.position);

        if (room != _currentRoom)
        {
            GoTo(room);
        }
    }

    private void EnterRoom(Vector2Int coord)
    {
        _rooms[coord].Enter();
    }

    private Vector2Int RoomAt(Vector3 worldPosition)
    {
        return new Vector2Int(
            Mathf.RoundToInt(worldPosition.x / LevelBuilder.ROOM_SIZE),
            Mathf.RoundToInt(worldPosition.y / LevelBuilder.ROOM_SIZE)
        );
    }

    private void SnapCameraTo(Vector2Int room)
    {
        camera.position = new Vector3(
            room.x * LevelBuilder.ROOM_SIZE,
            room.y * LevelBuilder.ROOM_SIZE,
            camera.position.z
        );
    }

    private void LoadFloor()
    {
        _rooms = levelBuilder.Build();

        foreach (var room in _rooms.Values)
        {
            room.Cleared += NextFloor;
        }

        GoTo(RoomAt(player.position));
    }

    private void NextFloor()
    {
        GameManager.Instance.NextFloor();
        levelBuilder.Clear();
        LoadFloor();
    }

    private void GoTo(Vector2Int room)
    {
        _currentRoom = room;
        SnapCameraTo(room);
        EnterRoom(room);
    }
}
