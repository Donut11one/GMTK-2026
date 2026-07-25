using UnityEngine;
using System.Collections.Generic;

public class LevelBuilder : MonoBehaviour
{
    [Header("Generation")]
    [SerializeField] private LevelGenerationSettings settings;

    [Header("References")]
    [SerializeField] private Room roomPrefab;
    [SerializeField] private Transform player;

    [Header("Layout")]
    [SerializeField] private float roomSize = 10f;

    private void Start()
    {
        var rng = new System.Random();
        var level = LevelGenerator.Generate(settings, rng);

        Build(level);
    }

    private void Build(LevelData level)
    {
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

            room.Build(openSides, roomSize);
            room.SetType(cell.Type);
        }

        player.position = ToWorld(level.Entrance);
    }

    private Vector3 ToWorld(Vector2Int coord)
    {
        return new Vector3(coord.x * roomSize, coord.y * roomSize, 0f);
    }
}
