using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class DungeonManager : MonoBehaviour, IRoomManager
{
    [SerializeField] private int _gridSize = 1;
    [SerializeField] private float _roomSize = 1.0f;
    [SerializeField] private float _linkSize = 0.1f;
    [SerializeField] private float _roomHeight = 1.0f;
    [SerializeField] private float _roomGenerationChance = 0.5f;

    [SerializeField] private Transform _floorPrefab = null;
    [SerializeField] private Transform _floorLinkPrefab = null;
    [SerializeField] private Transform _ceilPrefab = null;
    [SerializeField] private Transform _ceilLinkPrefab = null;
    [SerializeField] private Transform _wallPrefab = null;
    [SerializeField] private Transform _upWallPrefab = null;
    [SerializeField] private Transform _downWallPrefab = null;
    [SerializeField] private Transform _cornerPrefab = null;
    [SerializeField] private Transform _straightCornerPrefab = null;

    private Room[][] _dungeon = null;
    private Dictionary<Directions, Vector2Int> _directionToVector = null;
    private Dictionary<Directions, Directions> _inverseDirection = null;
    private Dictionary<RoomParts, Transform> _roomPartPrefabs = null;

    private System.Random rng = new System.Random();

    private List<(RoomParts, Transform)> _cornerPositions = null;
    private List<Transform> _linksPositions = null;

    public float RoomSize { get { return _roomSize; } }
    public float RoomHeight { get { return _roomHeight; } }

    private void Awake()
    {
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
        };

        _cornerPositions = new List<(RoomParts, Transform)>();
        _linksPositions = new List<Transform>();
        GenerateDungeon();
    }

    private void GenerateDungeon()
    {
        InitializeDungeon();
        FillDungeon();
        //GenerateCorners();
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

    public void InstantiateRoomPart(RoomParts part, Vector3 position, Quaternion rotation)
    {
        if (part == RoomParts.StraightCorner && _cornerPositions.Any(p => Vector3.SqrMagnitude(p.Item2.position - position) < 0.5f))
            return;

        if ((part == RoomParts.FloorLink || part == RoomParts.CeilLink) && _linksPositions.Any(p => Vector3.SqrMagnitude(p.position - position) < 0.5f))
            return;

        (RoomParts, Transform) otherCorner = _cornerPositions.Find(p => Vector3.SqrMagnitude(p.Item2.position - position) < 1.0f);
        if (part == RoomParts.Corner && otherCorner.Item2 != null && otherCorner.Item1 == RoomParts.StraightCorner)
        {
            _cornerPositions.Remove(otherCorner);
            Destroy(otherCorner.Item2.gameObject);
        }

        if (part == RoomParts.Wall)
        {
            Instantiate(_downWallPrefab, position, rotation);
            Instantiate(_wallPrefab, position + new Vector3(0.0f, _roomHeight / 3.0f, 0.0f), rotation);
            Instantiate(_upWallPrefab, position + new Vector3(0.0f, (_roomHeight / 3.0f) * 2.0f, 0.0f), rotation);
        }
        else
        {
            Transform newPart = Instantiate(_roomPartPrefabs[part], position, rotation);
            if (part == RoomParts.Corner || part == RoomParts.StraightCorner)
                _cornerPositions.Add((part, newPart));

            if (part == RoomParts.StraightCorner)
                newPart.position -= newPart.forward * 0.05f;

            if (part == RoomParts.FloorLink || part ==  RoomParts.CeilLink)
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
        throw new System.NotImplementedException();
    }

    public List<Directions> GetPossibleDirections(Vector2Int gridPosition)
    {
        throw new System.NotImplementedException();
    }
}
