using System.Collections.Generic;
using UnityEngine;


public enum RoomType { Normal, Entrance, Exit }

public class RoomCell
{
    public Vector2Int Coord;
    public RoomType Type = RoomType.Normal;
    public int EnemyCount;

    public RoomCell(Vector2Int coord)
    {
        Coord = coord;
    }
}

public class LevelData
{
    public static readonly Vector2Int[] Directions =
    {
        Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
    };
    // BL: technically redundant to have the coord in roomcell and the dictionary key
    // but makes it easier to deal with when we just want the coord and dont 
    // want to iterate over key value pairs
    public readonly Dictionary<Vector2Int, RoomCell> Rooms = new();
    public Vector2Int Entrance;
    public Vector2Int Exit;

    // 
    public bool HasRoomAt(Vector2Int coord) => Rooms.ContainsKey(coord);

    // BL: 1 connection = dead-end
    public int Connections(Vector2Int coord)
    {
        int count = 0;
        foreach (var dir in Directions)
            if (Rooms.ContainsKey(coord + dir)) count++;
        return count;
    }
}