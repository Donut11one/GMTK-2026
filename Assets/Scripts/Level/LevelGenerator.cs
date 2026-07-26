using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct LevelGenerationSettings
{
    public int gridWidth;
    public int gridHeight;
    public int targetRooms;
    public int minDeadEnds;
    public int maxEnemies;
}

public static class LevelGenerator
{
    private const int MaxAttempts = 50;

public static LevelData Generate(LevelGenerationSettings settings, System.Random rng)
{
    LevelData lastAttempt = null;

    for (int i = 0; i < MaxAttempts; i++)
    {
        lastAttempt = BuildLayout(settings, rng);
        var deadEnds = AssignTypes(lastAttempt, settings, rng);

        if (lastAttempt.Rooms.Count >= settings.targetRooms && 
            deadEnds.Count >= settings.minDeadEnds
        ){
            return lastAttempt;
        }
    }

    Debug.LogWarning($"LevelGenerator: couldn't satisfy targetRooms/minDeadEnds in " +
                     $"{MaxAttempts} tries. Returning best effort - try a bigger grid or fewer rooms.");

    return lastAttempt;
}

    // BL: BFS from the entrance that builds rooms as it grows
    private static LevelData BuildLayout(LevelGenerationSettings settings, System.Random rng)
    {
        var data = new LevelData();
        var start = new Vector2Int(settings.gridWidth / 2, settings.gridHeight / 2);
        data.Entrance = start;
        data.Rooms[start] = new RoomCell(start) { Type = RoomType.Entrance };

        var queue = new Queue<Vector2Int>();
        queue.Enqueue(start);
        bool placedThisPass = true;

        while (data.Rooms.Count < settings.targetRooms)
        {
            if (queue.Count == 0)
            {
                if (!placedThisPass)
                {
                    break;
                } 

                placedThisPass = false;

                foreach (var coord in data.Rooms.Keys.OrderBy(_ => rng.Next()).ToList())
                {
                    queue.Enqueue(coord);
                }                            
            }

            var cell = queue.Dequeue();

            foreach (var dir in LevelData.Directions.OrderBy(_ => rng.Next()))
            {
                if (data.Rooms.Count >= settings.targetRooms) 
                {
                    break;
                }

                var next = cell + dir;

                if (!CanPlace(data, next, settings, rng))
                {
                    continue;
                } 

                data.Rooms[next] = new RoomCell(next);
                queue.Enqueue(next);
                placedThisPass = true;
            }
        }
        
        return data;
    }

    private static bool CanPlace(LevelData data, Vector2Int cell, LevelGenerationSettings settings, System.Random rng)
    {
        if (
            cell.x < 0 || 
            cell.x >= settings.gridWidth || 
            cell.y < 0 || 
            cell.y >= settings.gridHeight
        ){
            return false;
        } 

        if (data.Rooms.ContainsKey(cell))
        {
            return false;
        } 

        // BL: 1 avoids loops, 2+ allows loops
        if (data.Connections(cell) > 1)
        {
            return false;
        }  

        // BL: coin flip to decide if a valid cell will be placeable for this level
        return rng.Next(2) == 0;                         
    }

    private static List<Vector2Int> AssignTypes(LevelData level, LevelGenerationSettings settings, System.Random rng)
    {
        foreach (var cell in level.Rooms.Values)
        {
            if (cell.Coord == level.Entrance)
            {
                cell.Type = RoomType.Entrance;
            }
            else
            {
                cell.Type = RoomType.Normal;
            }
        }
            
        var deadEnds = DeadEnds(level);

        if (deadEnds.Count == 0)
        {
            level.Exit = level.Entrance;
            return deadEnds;
        }

        level.Exit = deadEnds[0];
        level.Rooms[level.Exit].Type = RoomType.Exit;

        foreach (var cell in level.Rooms.Values)
        {
            if (cell.Type == RoomType.Normal)
            {
                cell.EnemyCount = rng.Next(1, settings.maxEnemies + 1);
            }
        }
        
        return deadEnds;
    }

    private static List<Vector2Int> DeadEnds(LevelData data)
    {
        var dist = Distances(data, data.Entrance);

        return data.Rooms.Keys
            .Where(coord => coord != data.Entrance && data.Connections(coord) == 1)
            .OrderByDescending(coord => dist[coord])
            .ToList();
    }

    // BL: BFS that measures path length to determine where exit should go
    private static Dictionary<Vector2Int, int> Distances(LevelData data, Vector2Int start)
    {
        var dist = new Dictionary<Vector2Int, int> { [start] = 0 };
        var queue = new Queue<Vector2Int>();
        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            var cell = queue.Dequeue();

            foreach (var dir in LevelData.Directions)
            {
                var neighbor = cell + dir;

                if (data.Rooms.ContainsKey(neighbor) && 
                    !dist.ContainsKey(neighbor)
                ){
                    dist[neighbor] = dist[cell] + 1;
                    queue.Enqueue(neighbor);
                }
            }
        }

        return dist;
    }
}