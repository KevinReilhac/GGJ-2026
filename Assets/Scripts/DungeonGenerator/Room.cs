using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room
{
    public bool IsEmpty { get; private set; } = true;
    public DungeonEvents DungeonEvent { get; private set; } = DungeonEvents.None;
    public List<Directions> OpenDirections { get; private set; } = null;

    private Vector3 _position = Vector3.zero;
    public Vector3 Position { get { return _position; } }

    public Room(Vector3 position)
    {
        _position = position;

        OpenDirections = new List<Directions>();
        IsEmpty = true;
        DungeonEvent = DungeonEvents.None;
    }

    public void FillRoom()
    {
        IsEmpty = false;
    }

    public void GenerateWalls(DungeonManager dungeonManager, Dictionary<Directions, Vector2Int> directionToVector)
    {
        if (IsEmpty)
            return;

        Vector3 upOffset = new Vector3(0.0f, dungeonManager.RoomHeight, 0.0f);

        dungeonManager.InstantiateRoomPart(RoomParts.Floor, _position, Quaternion.identity);
        dungeonManager.InstantiateRoomPart(RoomParts.Ceil, _position + upOffset, Quaternion.identity);

        List<Directions> directions = new List<Directions>()
        {
            Directions.Up,
            Directions.Right,
            Directions.Down,
            Directions.Left
        };
        for (int i = 0; i < 4; i++)
        {
            Directions direction = directions[i];
            Quaternion rotation = Quaternion.identity;
            if (direction == Directions.Left)
                rotation = Quaternion.Euler(0.0f, -90.0f, 0.0f);
            else if (direction == Directions.Right)
                rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
            else if (direction == Directions.Up)
                rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);

            Vector2Int offset = directionToVector[direction];
            Vector3 offsetPosition = _position + dungeonManager.GridToWorldDirection(offset, (dungeonManager.RoomSize + 0.3f) * 0.5f); ;
            if (OpenDirections.Contains(direction))
            {
                dungeonManager.InstantiateRoomPart(RoomParts.FloorLink, offsetPosition, rotation);
                dungeonManager.InstantiateRoomPart(RoomParts.CeilLink, offsetPosition + upOffset, rotation);
                Directions nextDirection = directions[(i + 1) % 4];
                offsetPosition += dungeonManager.GridToWorldDirection(directionToVector[nextDirection], (dungeonManager.RoomSize + 0.3f) * 0.5f);
                if (OpenDirections.Contains(nextDirection))
                {
                    //Corner
                    dungeonManager.InstantiateRoomPart(RoomParts.Corner, offsetPosition, Quaternion.identity);
                }
                else
                {
                    //Flat
                    if (rotation.eulerAngles.y == 90.0f || rotation.eulerAngles.y == 180.0f)
                        rotation *= Quaternion.Euler(0.0f, -90.0f, 0.0f);
                    dungeonManager.InstantiateRoomPart(RoomParts.StraightCorner, offsetPosition, rotation);
                }
            }
            else
            {
                dungeonManager.InstantiateRoomPart(RoomParts.Wall, offsetPosition, rotation);
                if (Random.Range(0.0f, 1.0f) < 0.2f)
                    dungeonManager.InstantiateRoomPart(RoomParts.Lantern, offsetPosition + Vector3.up, rotation);

                Directions nextDirection = directions[(i + 1) % 4];
                offsetPosition += dungeonManager.GridToWorldDirection(directionToVector[nextDirection], (dungeonManager.RoomSize + 0.3f) * 0.5f);
                if (OpenDirections.Contains(nextDirection))
                {
                    //Flat
                    if (rotation.eulerAngles.y == 90.0f)
                        rotation *= Quaternion.Euler(0.0f, -90.0f, 0.0f);
                    dungeonManager.InstantiateRoomPart(RoomParts.StraightCorner, offsetPosition, rotation);
                }
                else
                {
                    //Corner
                    dungeonManager.InstantiateRoomPart(RoomParts.Corner, offsetPosition, Quaternion.identity);
                }
            }
        }
    }

    public void Open(Directions direction)
    {
        if (!IsEmpty && !OpenDirections.Contains(direction))
            OpenDirections.Add(direction);
    }

    public void SetEvent(DungeonEvents dungeonEvent)
    {
        if (!IsEmpty && DungeonEvent == DungeonEvents.None)
            DungeonEvent = dungeonEvent;
    }

    public void DefeatEnemy()
    {
        if (DungeonEvent == DungeonEvents.Fight)
            DungeonEvent = DungeonEvents.None;
    }

    public override string ToString()
    {
        if (IsEmpty)
            return "_";

        switch (DungeonEvent)
        {
            case DungeonEvents.Fight:
                return "F";
            case DungeonEvents.Boss:
                return "B";
            case DungeonEvents.Chest:
                return "C";
            default:
                return "N";
        }
    }
}
