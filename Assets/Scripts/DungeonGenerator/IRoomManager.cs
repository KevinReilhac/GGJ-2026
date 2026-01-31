using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRoomManager
{
    public List<Directions> GetPossibleDirections(Vector2Int gridPosition);
    public DungeonEvents GetDungeonEvents(Vector2Int gridPosition);
    public int GetChestLoot(Vector2Int gridPosition);
}
