using UnityEngine;
using System.Collections.Generic;

public class LevelBuilder : MonoBehaviour
{
    [Header("Generation")]
    [SerializeField] private LevelGenerationSettings settings;

    [Header("References")]
    [SerializeField] private Room roomPrefab;
    [SerializeField] private Transform player;

    public const float ROOM_SIZE = 10f;

    public Dictionary<Vector2Int, Room> Build()
    {
        var rng = new System.Random();
        var level = LevelGenerator.Generate(settings, rng);
        var rooms = new Dictionary<Vector2Int, Room>();

        foreach (var cell in level.Rooms.Values)
        {
            var room = Instantiate(roomPrefab, ToWorld(cell.Coord), Quaternion.identity, transform);

            var openSides = new List<Vector2Int>();

            foreach (var direction in LevelData.Directions)
            {
                if (level.HasRoomAt(cell.Coord + direction))
                {
                    openSides.Add(direction);
                }
            }

            room.Build(openSides, ROOM_SIZE);
            room.SetType(cell.Type);
            room.SetEnemyCount(cell.EnemyCount);

            rooms[cell.Coord] = room;
        }

        player.position = ToWorld(level.Entrance);

        return rooms;
    }

    private Vector3 ToWorld(Vector2Int coord)
    {
        return new Vector3(coord.x * ROOM_SIZE, coord.y * ROOM_SIZE, 0f);
    }

    public void Clear()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            var room = transform.GetChild(i).gameObject;

            room.SetActive(false);
            Destroy(room);
        }
    }
}
