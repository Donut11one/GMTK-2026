using UnityEngine;
using System.Collections.Generic;
using System;

public class Room : MonoBehaviour
{
    [SerializeField] private GameObject shopPad;

    [Header("Walls")]
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject dooredWallPrefab;

    [Header("Floor Tiles")]
    [SerializeField] private float tileSize = 1f;
    [SerializeField] private int tileSortingOrder = -10;
    [SerializeField] private Sprite[] normalTiles;
    [SerializeField] private Sprite[] entranceTiles;
    [SerializeField] private Sprite[] exitTiles;
    [SerializeField] private Sprite[] shopTiles;

    [Header("Enemies")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnRange = 4f;

    // BL: simplest way to prevent enemies from overlapping each other or player is spawn in corners
    // only works while there are no obstacles in rooms and max enemies is 4. time crunch n all that
    private static readonly Vector3[] SpawnCorners =
    {
        new Vector3(1f, 1f, 0f),
        new Vector3(-1f, 1f, 0f),
        new Vector3(-1f, -1f, 0f),
        new Vector3(1f, -1f, 0f)
    };

    public event Action Cleared;
    public RoomType Type { get; private set; }

    private readonly List<Door> _doors = new();
    private readonly List<GameObject> _enemies = new();
    private float _roomSize;
    private int _enemyCount;
    private bool _explored;
    private bool _inCombat;

    public void Build(ICollection<Vector2Int> openSides, float roomSize)
    {
        _roomSize = roomSize;

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

    public void SetEnemyCount(int count)
    {
        _enemyCount = count;
    }

    public void Enter()
    {
        if (_explored)
        {
            return;
        }

        _explored = true;
        SpawnEnemies();

        if (_enemyCount > 0)
        {
            SetDoorsOpen(false);
            _inCombat = true;
        }
    }

    private void SpawnEnemies()
    {
        for (int i = 0; i < _enemyCount; i++)
        {
            _enemies.Add(Instantiate(enemyPrefab, transform.position + SpawnCorners[i] * spawnRange, Quaternion.identity));
        }
    }

    private void Update()
    {
        if (!_inCombat)
        {
            return;
        }

        _enemies.RemoveAll(enemy => enemy == null);

        if (_enemies.Count == 0)
        {
            _inCombat = false;

            if (Type == RoomType.Normal)
            {
                SetDoorsOpen(true);
            }
            else
            {
                Cleared?.Invoke();
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
        Type = type;
        BuildFloor(TilesFor(type));
        shopPad.SetActive(type == RoomType.Shop);
    }

    private Sprite[] TilesFor(RoomType type)
    {
        if (type == RoomType.Entrance)
        {
            return entranceTiles;
        }

        if (type == RoomType.Exit)
        {
            return exitTiles;
        }

        if (type == RoomType.Shop)
        {
            return shopTiles;
        }

        return normalTiles;
    }

    private void BuildFloor(Sprite[] tiles)
    {
        if (tiles == null || tiles.Length == 0)
        {
            return;
        }

        int count = Mathf.RoundToInt(_roomSize / tileSize);
        float origin = (tileSize - _roomSize) / 2f;

        for (int x = 0; x < count; x++)
        {
            for (int y = 0; y < count; y++)
            {
                var tile = new GameObject("Tile");
                tile.transform.SetParent(transform, false);
                tile.transform.localPosition = new Vector3(origin + x * tileSize, origin + y * tileSize, 0f);

                var sprite = tile.AddComponent<SpriteRenderer>();
                sprite.sprite = tiles[UnityEngine.Random.Range(0, tiles.Length)];
                sprite.sortingOrder = tileSortingOrder;
            }
        }
    }
}
