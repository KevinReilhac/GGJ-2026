using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonManager : MonoBehaviour, IRoomManager
{
    [SerializeField] private int _gridSize = 1;
    [SerializeField] private float _roomSize = 1.0f;
    [SerializeField] private float _linkSize = 0.1f;
    [SerializeField] private float _roomHeight = 1.0f;
    [SerializeField] private float _roomGenerationChance = 0.5f;
    [SerializeField] private float _directionChangeChance = 0.5f;
    [SerializeField] private float _fightChance = 0.5f;
    [SerializeField] private float _chestChance = 0.5f;

    [SerializeField] private Transform _floorPrefab = null;
    [SerializeField] private Transform _floorLinkPrefab = null;
    [SerializeField] private Transform _ceilPrefab = null;
    [SerializeField] private Transform _ceilLinkPrefab = null;
    [SerializeField] private Transform _wallPrefab = null;
    [SerializeField] private Transform _upWallPrefab = null;
    [SerializeField] private Transform _downWallPrefab = null;
    [SerializeField] private Transform _cornerPrefab = null;
    [SerializeField] private Transform _straightCornerPrefab = null;
    [SerializeField] private Transform _chestPrefab = null;
    [SerializeField] private Transform _fightPrefab = null;
    [SerializeField] private Transform _stairsPrefab = null;
    [SerializeField] private Transform _lanternPrefab = null;

    private Room[][] _dungeon = null;
    private Dictionary<Directions, Vector2Int> _directionToVector = null;
    private Dictionary<Directions, Directions> _inverseDirection = null;
    private Dictionary<RoomParts, Transform> _roomPartPrefabs = null;
    private Vector3 _startPosition = Vector3.zero;
    private Vector2Int _startGridPosition = Vector2Int.zero;

    private System.Random rng = new System.Random();

    private List<(RoomParts, Transform)> _cornerPositions = null;
    private List<Transform> _linksPositions = null;
    private List<Transform> _wallPositions = null;

    public float RoomSize { get { return _roomSize; } }
    public float RoomHeight { get { return _roomHeight; } }

    private Vector2Int _currentFightPosition = Vector2Int.zero;
    private DungeonEvents _currentFightType = DungeonEvents.None;

    Dictionary<Vector2Int, Transform> _enemies = null;

    private void Awake()
    {
#if !UNITY_EDITOR
        FoxEdit.VoxelSharedData.Initialize();
#endif

        _directionToVector = new Dictionary<Directions, Vector2Int>
        {
            [Directions.Up] = new Vector2Int(0, 1),
            [Directions.Down] = new Vector2Int(0, -1),
            [Directions.Right] = new Vector2Int(1, 0),
            [Directions.Left] = new Vector2Int(-1, 0)
        };

        _inverseDirection = new Dictionary<Directions, Directions>
        {
            [Directions.Up] = Directions.Down,
            [Directions.Down] = Directions.Up,
            [Directions.Right] = Directions.Left,
            [Directions.Left] = Directions.Right
        };

        _roomPartPrefabs = new Dictionary<RoomParts, Transform>
        {
            [RoomParts.Floor] = _floorPrefab,
            [RoomParts.FloorLink] = _floorLinkPrefab,
            [RoomParts.Ceil] = _ceilPrefab,
            [RoomParts.CeilLink] = _ceilLinkPrefab,
            [RoomParts.Wall] = _wallPrefab,
            [RoomParts.Corner] = _cornerPrefab,
            [RoomParts.StraightCorner] = _straightCornerPrefab,
            [RoomParts.Chest] = _chestPrefab,
            [RoomParts.Fight] = _fightPrefab,
            [RoomParts.Stairs] = _stairsPrefab,
            [RoomParts.Lantern] = _lanternPrefab,
        };

        _cornerPositions = new List<(RoomParts, Transform)>();
        _linksPositions = new List<Transform>();
        _wallPositions = new List<Transform>();
        _enemies = new Dictionary<Vector2Int, Transform>();
        GenerateDungeon();
    }

    private void Start()
    {
        FightManager.OnWinFight += OnWinFight;
    }

    private void OnWinFight(Fight fight)
    {
        if (_currentFightType == DungeonEvents.Fight)
        {
            Room room = GetRoom(_currentFightPosition);
            room?.DefeatEnemy();
            Destroy(_enemies[_currentFightPosition]?.gameObject);
            _enemies.Remove(_currentFightPosition);
        }
        else if (_currentFightType == DungeonEvents.Boss)
        {
            _cornerPositions = new List<(RoomParts, Transform)>();
            _linksPositions = new List<Transform>();
            _wallPositions = new List<Transform>();
            _enemies = new Dictionary<Vector2Int, Transform>();
            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Transform child = transform.GetChild(i);
                Destroy(child.gameObject);
            }
            GenerateDungeon();
            FindObjectOfType<MovingManager>().Teleport();
        }
    }

    private void OnDestroy()
    {
        FightManager.OnWinFight -= OnWinFight;
    }

    private void GenerateDungeon()
    {
        InitializeDungeon();
        FillDungeon();
        //GenerateCorners();
        AddEvents();
    }

    private void AddEvents()
    {
        float maxDistance = -1.0f;
        Vector2Int bossPosition = Vector2Int.zero;
        for (int y = 0; y < _gridSize; y++)
        {
            for (int x = 0; x < _gridSize; x++)
            {
                Vector2Int position = new Vector2Int(x, y);

                if (!(x == 0 || x == _gridSize - 1 || y == 0 || y == _gridSize - 1))
                    continue;

                Room room = GetRoom(position);
                if (room != null && !room.IsEmpty)
                {
                    float distance = Vector2Int.Distance(position, _startGridPosition);
                    if (distance > maxDistance)
                    {
                        maxDistance = distance;
                        bossPosition = position;
                    }
                }
            }
        }

        Room bossRoom = GetRoom(bossPosition);
        bossRoom.SetEvent(DungeonEvents.Boss);
        InstantiateRoomPart(RoomParts.Fight, bossRoom.Position, Quaternion.identity);
        Directions stairsDirection = _inverseDirection[bossRoom.OpenDirections.First()];
        Vector2Int stairsGridPosition = bossPosition + _directionToVector[stairsDirection];
        Vector3 stairsPosition = GridToWorldPosition(stairsGridPosition, _roomSize + _linkSize);
        Transform wall = _wallPositions.Find(w =>
        {
            Vector3 wPos = w.position;
            wPos.y = 0.0f;
            return Vector3.SqrMagnitude(wPos - stairsPosition) < 1.0f;
        });

        Destroy(wall.gameObject);
        Quaternion stairsRotation = Quaternion.identity;
        if (stairsDirection == Directions.Up)
            stairsRotation = Quaternion.Euler(0.0f, -90.0f, 0.0f);
        else if (stairsDirection == Directions.Down)
            stairsRotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
        else if (stairsDirection == Directions.Right)
            stairsRotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        InstantiateRoomPart(RoomParts.Stairs, stairsPosition, stairsRotation);

        for (int y = 0; y < _gridSize; y++)
        {
            for (int x = 0; x < _gridSize; x++)
            {
                Vector2Int position = new Vector2Int(x, y);
                Room room = GetRoom(position);
                if (room == null || room.IsEmpty || position == _startGridPosition || room.DungeonEvent != DungeonEvents.None)
                    continue;

                int adjecentRoomCount = GetAdjacentRoomCount(position);

                if (Random.Range(0.0f, 1.0f) < _fightChance && position != _startGridPosition)
                {
                    room.SetEvent(DungeonEvents.Fight);
                    InstantiateRoomPart(RoomParts.Fight, room.Position, Quaternion.identity, position);
                    continue;
                }

                //if (adjecentRoomCount == 1)
                //{
                //    foreach (Directions direction in _directionToVector.Keys)
                //    {
                //        Vector2Int vectorDirection = _directionToVector[direction];
                //        Room adjacentRoom = GetRoom(position + vectorDirection);
                //        if (adjacentRoom != null && !adjacentRoom.IsEmpty && adjacentRoom.DungeonEvent == DungeonEvents.None && position + vectorDirection != _startGridPosition
                //            && GetAdjacentRoomCount(position + vectorDirection) == 2 && Random.Range(0.0f, 1.0f) < _chestChance)
                //        {
                //            Quaternion rotation = Quaternion.identity;
                //            if (direction == Directions.Up)
                //                rotation = Quaternion.Euler(0.0f, -90.0f, 0.0f);
                //            else if (direction == Directions.Down)
                //                rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
                //            else if (direction == Directions.Left)
                //                rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                //            room.SetEvent(DungeonEvents.Chest);
                //            InstantiateRoomPart(RoomParts.Chest, room.Position, rotation);
                //            adjacentRoom.SetEvent(DungeonEvents.Fight);
                //            InstantiateRoomPart(RoomParts.Fight, adjacentRoom.Position, rotation);
                //            break;
                //        }
                //    }
                //}
            }
        }
    }

    private void GenerateCorners()
    {
        int[] horizontalCorners = new int[_gridSize + 1];
        for (int y = 0; y < _gridSize; y++)
        {
            for (int x = 0; x < _gridSize + 1; x++)
            {
                Room room = GetRoom(new Vector2Int(x, y));
                if (room != null && !room.IsEmpty)
                {
                    int inverseY = _gridSize - y - 1;
                    horizontalCorners[inverseY] |= 1 << x;
                    horizontalCorners[inverseY] |= 1 << (x + 1);
                    horizontalCorners[inverseY + 1] |= 1 << x;
                    horizontalCorners[inverseY + 1] |= 1 << (x + 1);
                }
            }
        }

        int[] verticalCorners = new int[_gridSize + 1];
        for (int x = 0; x < _gridSize; x++)
        {
            for (int y = 0; y < _gridSize + 1; y++)
            {
                Room room = GetRoom(new Vector2Int(x, y));
                if (room != null && !room.IsEmpty)
                {
                    verticalCorners[x] |= 1 << y;
                    verticalCorners[x] |= 1 << (y + 1);
                    verticalCorners[x + 1] |= 1 << y;
                    verticalCorners[x + 1] |= 1 << (y + 1);
                }
            }
        }

        //for (int i = 0; i < _gridSize + 1; i++)
        //{
        //    horizontalCorners[i] = (horizontalCorners[i] << 1) & (horizontalCorners[i] >> 1);
        //    verticalCorners[i] = (verticalCorners[i] << 1) & (verticalCorners[i] >> 1);
        //}

        //int[] rotatedVerticalCorners = new int[_gridSize + 1];
        //for (int x = 0; x < _gridSize + 1; x++)
        //{
        //    for (int y = 0; y < _gridSize + 1; y++)
        //    {
        //        rotatedVerticalCorners[_gridSize - y] |= ((verticalCorners[x] >> y) & 1) << x;
        //    }
        //}

        //int[] angleCorners = new int[_gridSize + 1];
        //int[] straightCorners = new int[_gridSize + 1];
        //for (int i = 0; i < _gridSize + 1; i++)
        //{
        //    angleCorners[i] = horizontalCorners[i] & rotatedVerticalCorners[i];
        //    int mergedCorners = horizontalCorners[i] | rotatedVerticalCorners[i];
        //    straightCorners[i] = angleCorners[i] ^ mergedCorners;
        //}

        string result = "";
        for (int i = _gridSize; i >= 0; i--)
        {
            result += horizontalCorners[i].ToBinaryString() + "\n";
        }

        result += "\n";
        for (int i = _gridSize; i >= 0; i--)
        {
            result += verticalCorners[i].ToBinaryString() + "\n";
        }

        Debug.Log(result);
    }

    private void InitializeDungeon()
    {
        _dungeon = new Room[_gridSize][];
        for (int y = 0; y < _gridSize; y++)
        {
            _dungeon[y] = new Room[_gridSize];
            for (int x = 0; x < _gridSize; x++)
            {
                _dungeon[y][x] = new Room(GridToWorldPosition(new Vector2Int(x, y), _roomSize + _linkSize));
            }
        }
    }

    public Vector3 GridToWorldPosition(Vector2Int gridPosition, float multiplier = 1.0f)
    {
        return new Vector3(gridPosition.x * multiplier, 0.0f, (_gridSize - gridPosition.y - 1) * multiplier);
    }

    public Vector3 GridToWorldDirection(Vector2Int gridDirection, float multiplier = 1.0f)
    {
        return new Vector3(gridDirection.x * multiplier, 0.0f, -gridDirection.y * multiplier);
    }

    private void FillDungeon()
    {
        Vector2Int startPosition = new Vector2Int
        {
            x = Random.Range(0, _gridSize),
            y = 0
        };

        _startPosition = GridToWorldPosition(startPosition, _roomSize + _linkSize);
        _startGridPosition = startPosition;

        CreateRoom(startPosition, Directions.None, true);
        CreateWalls();
    }

    private void CreateWalls()
    {
        for (int y = 0; y < _gridSize; y++)
        {
            for (int x = 0; x < _gridSize; x++)
            {
                _dungeon[y][x].GenerateWalls(this, _directionToVector);
            }
        }
    }

    public void InstantiateRoomPart(RoomParts part, Vector3 position, Quaternion rotation, Vector2Int gridPosition = default)
    {
        if (part == RoomParts.StraightCorner && _cornerPositions.Any(p => Vector3.SqrMagnitude(p.Item2.position - position) < 0.5f))
            return;

        if ((part == RoomParts.FloorLink || part == RoomParts.CeilLink) && _linksPositions.Any(p => Vector3.SqrMagnitude(p.position - position) < 0.5f))
            return;

        if (part == RoomParts.Fight)
        {
            Transform enemy = Instantiate(_roomPartPrefabs[part], position, rotation);
            _enemies[gridPosition] = enemy;
            enemy.parent = transform;
            return;
        }

        (RoomParts, Transform) otherCorner = _cornerPositions.Find(p => Vector3.SqrMagnitude(p.Item2.position - position) < 1.0f);
        if (part == RoomParts.Corner && otherCorner.Item2 != null && otherCorner.Item1 == RoomParts.StraightCorner)
        {
            _cornerPositions.Remove(otherCorner);
            Destroy(otherCorner.Item2.gameObject);
        }

        if (part == RoomParts.Wall)
        {
            Transform wall = Instantiate(_wallPrefab, position + new Vector3(0.0f, _roomHeight / 3.0f, 0.0f), rotation);
            Transform downWall = Instantiate(_downWallPrefab, position, rotation);
            downWall.parent = wall;
            Transform upWall = Instantiate(_upWallPrefab, position + new Vector3(0.0f, (_roomHeight / 3.0f) * 2.0f, 0.0f), rotation);
            upWall.parent = wall;

            _wallPositions.Add(wall);
            wall.parent = transform;
        }
        else
        {
            Transform newPart = Instantiate(_roomPartPrefabs[part], position, rotation);
            newPart.parent = transform;
            if (part == RoomParts.Corner || part == RoomParts.StraightCorner)
                _cornerPositions.Add((part, newPart));

            if (part == RoomParts.StraightCorner)
                newPart.position -= newPart.forward * 0.05f;

            if (part == RoomParts.FloorLink || part == RoomParts.CeilLink)
                _linksPositions.Add(newPart);
        }
    }

    private bool CreateRoom(Vector2Int position, Directions incomingDirection, bool canChangeGenerationDirection)
    {
        if (!IsInBound(position))
            return false;

        Room room = GetRoom(position);
        if (room == null || !room.IsEmpty)
            return false;

        if (GetAdjacentRoomCount(position) > 1)
            return false;

        if (!(Random.Range(0.0f, 1.0f) < _roomGenerationChance) && incomingDirection != Directions.None)
            return false;

        room.FillRoom();
        if (incomingDirection != Directions.None)
            room.Open(_inverseDirection[incomingDirection]);

        List<Directions> keys = _directionToVector.Keys.ToList();
        keys.OrderBy(_ => rng.Next()).ToList();

        foreach (Directions direction in keys)
        {
            if (direction == incomingDirection && Random.Range(0.0f, 1.0f) < _directionChangeChance)
                continue;

            Vector2Int offset = _directionToVector[direction];
            if (!(direction == Directions.None || canChangeGenerationDirection || incomingDirection == direction))
                continue;

            if (CreateRoom(position + offset, direction, !canChangeGenerationDirection || incomingDirection != direction))
                room.Open(direction);
        }

        return true;
    }

    private int GetAdjacentRoomCount(Vector2Int position)
    {
        int count = 0;

        foreach (Vector2Int direction in _directionToVector.Values)
        {
            Vector2Int adjacentPosition = position + direction;
            if (IsInBound(adjacentPosition) && !GetRoom(adjacentPosition).IsEmpty)
                count += 1;
        }

        return count;
    }

    private Room GetRoom(Vector2Int position)
    {
        if (IsInBound(position))
            return _dungeon[position.y][position.x];
        return null;
    }

    private bool IsInBound(Vector2Int position)
    {
        if (position.x >= _gridSize || position.x < 0 || position.y >= _gridSize || position.y < 0)
            return false;
        return true;
    }

    private void PrintDungeon()
    {
        string result = "";
        for (int y = 0; y < _gridSize; y++)
        {
            for (int x = 0; x < _gridSize; x++)
            {
                result += _dungeon[y][x].ToString();
            }
            result += "\n";
        }
        Debug.Log(result);
    }

    public int GetChestLoot(Vector2Int gridPosition)
    {
        throw new System.NotImplementedException();
    }

    public DungeonEvents GetDungeonEvents(Vector2Int gridPosition)
    {
        Room room = GetRoom(gridPosition);
        if (room != null)
        {
            if (room.DungeonEvent == DungeonEvents.Fight)
            {
                _currentFightType = DungeonEvents.Fight;
                _currentFightPosition = gridPosition;
            }
            else if (room.DungeonEvent == DungeonEvents.Boss)
            {
                _currentFightType = DungeonEvents.Boss;
            }
            return room.DungeonEvent;
        }
        return DungeonEvents.None;
    }

    public List<Directions> GetPossibleDirections(Vector2Int gridPosition)
    {
        Room room = GetRoom(gridPosition);
        if (room != null)
        {
            List<Directions> inverseOpen = new List<Directions>(room.OpenDirections);
            for (int i = 0; i < inverseOpen.Count; i++)
            {
                if (inverseOpen[i] == Directions.Up || inverseOpen[i] == Directions.Down)
                    inverseOpen[i] = _inverseDirection[inverseOpen[i]];
            }
            return inverseOpen;
        }
        return new List<Directions>();
    }

    public Vector3 GetStartPosition(out Vector2Int gridPosition)
    {
        gridPosition = _startGridPosition;
        return _startPosition;
    }
}
