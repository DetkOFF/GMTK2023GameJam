using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadTile: MonoBehaviour
{
    public Vector3Int Pos;
    public RoadTile NextTile;
    public TileDirection NextTileDirection;
    public RoadTileType RoadType;
    public RoadTile(Vector3Int pos)
    {
        Pos = pos;
        NextTile = null;
        NextTileDirection = TileDirection.None;
        RoadType = RoadTileType.Default;
    }
    /*
    public RoadTile Left;
    public RoadTile Right;
    public RoadTile Top;
    public RoadTile Down;
    public bool hasNextTile;
    
    */
}

public enum RoadTileType
{
    Default,
    SpawnPoint,
    Destination,
}
