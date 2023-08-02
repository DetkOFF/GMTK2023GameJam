using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using NaughtyAttributes;
using Unity.VisualScripting;

public class LevelBuilder : MonoBehaviour
{
    [SerializeField] private Tilemap roadsTileMap;
    [SerializeField] private TileBase DestinationTile;
    //[SerializeField] private BoundsInt roadsTileMapSize;
    //[SerializeField] private Vector2Int roadsSize;

    [SerializeField] private GameObject roadPrefab;
    [SerializeField] private GameObject startPosPrefab;
    [SerializeField] private GameObject destinationPrefab;
    [SerializeField] private Transform roadsParent;

    //[SerializeField] private List<RoadTile> roadTiles = new List<RoadTile>();

    [Button("Build Map")]
    public void BuildMap()
    {
        FindPaths();
        Debug.Log("---------Build Complete!---------");
    }

    private void Start()
    {
        //BuildMap();
    }

    public Vector2Int[] startPositions;

    [SerializeField] private List<RoadTile> DestinationTiles;
    /*private void ReadRoods()
    {
        
        foreach (var destinationPos in destinationPositions)
        {
            //RecursionPathFinder()
        }
        
        foreach (var startPos in startPositions)
        {
            
        }
        
        
        
        var tiles = roadsTileMap.GetTilesBlock(roadsTileMapSize);
        roadTiles.Clear();
        int[][] temp;
        for (int i = 0; i < roadsSize.x; i++)
        {
            for (int j = 0; j < roadsSize.y; j++)
            {
                var currentPos = new Vector3Int(i, j, 0);
                
                if (roadsTileMap.GetTile(currentPos) != null)
                {
                    RoadTile rTile = new RoadTile(new Vector2Int(currentPos.x, currentPos.y));
                    if (roadsTileMap.GetTile(currentPos + Vector3Int.right) != null)
                    {
                        
                    }
                    if (roadsTileMap.GetTile(currentPos + Vector3Int.left) != null)
                    {
                        
                    }
                    if (roadsTileMap.GetTile(currentPos + Vector3Int.up) != null)
                    {
                        
                    }
                    if (roadsTileMap.GetTile(currentPos + Vector3Int.down) != null)
                    {
                        
                    }
                    roadTiles.Add(rTile);
                }
            }
        }
        
    }*/


    private int StartPositionsCount = 0;

    [Button("Find Destination Tiles")]
    public bool FindDestinationTiles()
    {
        bool found = false;
        DestinationTiles = new List<RoadTile>();
        //roadsTileMap.GetCellCenterWorld()

        for (int x = -50; x < 50; x++)
            for (int y = -50; y < 50; y++)
                if (roadsTileMap.GetTile(new Vector3Int(x, y, 0)) == DestinationTile)
                {
                    found = true;
                    var dest = (Instantiate(destinationPrefab, roadsTileMap.GetCellCenterWorld(new Vector3Int(x, y, 0)), roadsParent.rotation, roadsParent)).GetComponent<RoadTile>();
                    dest.Pos = new Vector3Int(x, y, 0);
                    DestinationTiles.Add(dest);
                    Debug.Log(x + "; " + y);
                }
        return found;
    }
    private RoadTile RecursionPathFinder(RoadTile currentTile)
    {
        bool isDeadEnd = true;
        if (currentTile.NextTileDirection != TileDirection.Right && roadsTileMap.GetTile( currentTile.Pos + Vector3Int.right) != null)
        {
            isDeadEnd = false;
           // var next = roadsTileMap.GetTile(currentTile.Pos + Vector3Int.right);
            var tileObject = Instantiate(roadPrefab, roadsTileMap.GetCellCenterWorld((currentTile.Pos + Vector3Int.right)), roadsParent.rotation, roadsParent);
            var previous = tileObject.GetComponent<RoadTile>();
            previous.NextTile = currentTile;
            previous.Pos = currentTile.Pos + Vector3Int.right;
            previous.NextTileDirection = TileDirection.Left;
            RecursionPathFinder(previous);
        }
        if (currentTile.NextTileDirection != TileDirection.Left && roadsTileMap.GetTile(currentTile.Pos + Vector3Int.left) != null)
        {
            isDeadEnd = false;
            var tileObject = Instantiate(roadPrefab, roadsTileMap.GetCellCenterWorld((currentTile.Pos + Vector3Int.left)), roadsParent.rotation, roadsParent);
            var previous = tileObject.GetComponent<RoadTile>();
            previous.NextTile = currentTile;
            previous.Pos = currentTile.Pos + Vector3Int.left;
            previous.NextTileDirection = TileDirection.Right;
            RecursionPathFinder(previous);           
        }
        if (currentTile.NextTileDirection != TileDirection.Top && roadsTileMap.GetTile(currentTile.Pos + Vector3Int.up) != null)
        {
            isDeadEnd = false;
            var tileObject = Instantiate(roadPrefab, roadsTileMap.GetCellCenterWorld((currentTile.Pos + Vector3Int.up)), roadsParent.rotation, roadsParent);
            var previous = tileObject.GetComponent<RoadTile>();
            previous.NextTile = currentTile;
            previous.Pos = currentTile.Pos + Vector3Int.up;
            previous.NextTileDirection = TileDirection.Down;
            RecursionPathFinder(previous);             
        }
        if (currentTile.NextTileDirection != TileDirection.Down && roadsTileMap.GetTile(currentTile.Pos + Vector3Int.down) != null)
        {
            isDeadEnd = false;
            var tileObject = Instantiate(roadPrefab, roadsTileMap.GetCellCenterWorld((currentTile.Pos + Vector3Int.down)), roadsParent.rotation, roadsParent);
            var previous = tileObject.GetComponent<RoadTile>();
            previous.NextTile = currentTile;
            previous.Pos = currentTile.Pos + Vector3Int.down;
            previous.NextTileDirection = TileDirection.Top;
            RecursionPathFinder(previous);              
        }

        if (isDeadEnd)
        {
            StartPositionsCount++;
            currentTile.RoadType = RoadTileType.SpawnPoint;
            currentTile.AddComponent<BoxCollider2D>();
            Instantiate(startPosPrefab, roadsTileMap.GetCellCenterWorld(currentTile.Pos), roadsParent.rotation, roadsParent);
        }
        return null;
    }
    
    private bool FindPaths()
    {
        StartPositionsCount = 0;
        RecursionPathFinder(DestinationTiles[0]);
        /*foreach (var destination in DestinationTiles)
        {
            RecursionPathFinder(destination);
        }*/
        Debug.Log("found "+StartPositionsCount+" start positions");
        return true;
    }

}



public enum TileDirection
{
    Left,
    Right,
    Top,
    Down,
    None,
}