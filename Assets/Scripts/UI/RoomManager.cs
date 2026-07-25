using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform camera;

    private Vector2Int _currentRoom;

    private void Start()
    {
        _currentRoom = RoomAt(player.position);
        SnapCameraTo(_currentRoom);
    }

    private void Update()
    {
        var room = RoomAt(player.position);

        if (room != _currentRoom)
        {
            SnapCameraTo(room);
        }
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
}
