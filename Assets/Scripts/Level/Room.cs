using UnityEngine;
using System.Collections.Generic;

public class Room : MonoBehaviour
{
    [Header("Walls")]
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject dooredWallPrefab;

    [Header("Room Colors")]
    [SerializeField] private SpriteRenderer floor;
    [SerializeField] private Color normalColor = Color.grey;
    [SerializeField] private Color entranceColor = Color.green;
    [SerializeField] private Color exitColor = Color.red;

    private readonly List<Door> _doors = new();

    public void Build(ICollection<Vector2Int> openSides, float roomSize)
    {
        foreach (var direction in LevelData.Directions)
        {
            bool isDoorWay = openSides.Contains(direction);
            var prefab = isDoorWay
                ? dooredWallPrefab
                : wallPrefab;

            var piece = Instantiate(prefab, transform);
            piece.transform.localPosition = SidePosition(direction, roomSize);
            piece.transform.localRotation = SideRotation(direction);

            if (isDoorWay)
            {
                RegisterDoor(piece);
            }
        }
    }

    public void SetDoorsOpen(bool open)
    {
        foreach (var door in _doors)
        {
            door.OpenState(open);
        }
    }

    private void RegisterDoor(GameObject doorWall)
    {
        var door = doorWall.GetComponentInChildren<Door>();

        if (door != null)
        {
            door.OpenState(true);
            _doors.Add(door);
        }
    }

    private Vector3 SidePosition(Vector2Int direction, float roomSize)
    {
        float half = roomSize / 2f;
        return new Vector3(direction.x * half, direction.y * half, 0f);
    }

    private Quaternion SideRotation(Vector2Int direction)
    {
        bool vertical = direction.x != 0;

        if (vertical)
        {
            return Quaternion.Euler(0f, 0f, 90f);
        }

        return Quaternion.identity;
    }

    public void SetType(RoomType type)
    {
        floor.color = ColorFor(type);
    }

    private Color ColorFor(RoomType type)
    {
        if (type == RoomType.Entrance)
        {
            return entranceColor;
        }

        if (type == RoomType.Exit)
        {
            return exitColor;
        }

        return normalColor;
    }
}
