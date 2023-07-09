using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    [SerializeField] private Transform _ground;
    private Vector2Int _size;
    [SerializeField] private GameTile _tilePrefab;

    private GameTileContentFactory _contentFactory;

    private GameTile[] _tiles;

    private Queue<GameTile> _searchFrontier = new Queue<GameTile>();

    private List<GameTile> _spawnPoints = new List<GameTile>();
    private List<GameTileContent> _contentToUpdate = new List<GameTileContent>();
    public void Initialize(Vector2Int size, GameTileContentFactory contentFactory)
    {
        _size = size;
        _ground.localScale = new Vector3(size.x, size.y, 1f);

        Vector2 offset = new Vector2((size.x - 1)*0.5f, (size.y - 1)*0.5f);
        _tiles = new GameTile[size.x * size.y];
        _contentFactory = contentFactory;
        for (int i = 0, y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++, i++)
            {
                GameTile tile = _tiles[i] = Instantiate(_tilePrefab);
                tile.transform.SetParent(transform, false);
                tile.transform.localPosition = new Vector3(x - offset.x, y - offset.y,0f);

                if (x > 0)
                {
                    GameTile.MakeEastWestNeighbors(tile, _tiles[i-1]);
                }
                if (y > 0)
                {
                    GameTile.MakeNorthSouthNeighbors(tile, _tiles[i-size.x]);
                }

                tile.IsAlternative = (x & 1) == 0;
                if ((y & 1) == 0)
                {
                    tile.IsAlternative = !tile.IsAlternative;
                }
                tile.Content = _contentFactory.Get(GameTileContentType.Empty);
            }
        }
        //ToggleDestination(_tiles[_tiles.Length / 2]);
        //ToggleSpawnPoint(_tiles[0]);
    }

    public void GameUpdate()
    {
        for (int i = 0; i < _contentToUpdate.Count; i++)
        {
            _contentToUpdate[i].GameUpdate();
        }
    }

    public bool FindPaths()
    {
        foreach (var tile in _tiles)
        {
            if(tile.Content.Type == GameTileContentType.Destination)
            {
                tile.BecomeDestination();
                _searchFrontier.Enqueue(tile);
            }
            else
            {
                tile.ClearPath();
            }
        }

        if(_searchFrontier.Count == 0)
        {
            return false;
        }

        //int destinationIndex = _tiles.Length / 2;
        //_tiles[destinationIndex].BecomeDestination();
        //_searchFrontier.Enqueue(_tiles[destinationIndex]);

        while (_searchFrontier.Count > 0)
        {
            GameTile tile = _searchFrontier.Dequeue();
            if (tile != null)
            {
                if (tile.IsAlternative)
                {
                    _searchFrontier.Enqueue(tile.GrowPathNorth());
                    _searchFrontier.Enqueue(tile.GrowPathSouth());
                    _searchFrontier.Enqueue(tile.GrowPathEast());
                    _searchFrontier.Enqueue(tile.GrowPathWest());
                }
                else
                {
                    _searchFrontier.Enqueue(tile.GrowPathWest());
                    _searchFrontier.Enqueue(tile.GrowPathEast());
                    _searchFrontier.Enqueue(tile.GrowPathSouth());
                    _searchFrontier.Enqueue(tile.GrowPathNorth());
                }
                
            }
        }
        foreach(var t in _tiles)
        {
            if(!t.HasPath)
            {
                return false;
            }
        }
        foreach (var t in _tiles)
        {
            t.ShowPath();
        }

        return true;
    }

    public void ToggleDestination(GameTile tile)
    {
        if(tile.Content.Type == GameTileContentType.Destination)
        {
            tile.Content = _contentFactory.Get(GameTileContentType.Empty);
            if(!FindPaths())
            {
                tile.Content = _contentFactory.Get(GameTileContentType.Destination);
                FindPaths();
            }
            FindPaths();
        }
        else if (tile.Content.Type ==  GameTileContentType.Empty)
        {
            tile.Content = _contentFactory.Get(GameTileContentType.Destination);
            FindPaths();
        }
        FindPaths();
    }
    public void ToggleSpawnPoint(GameTile tile)
    {
        if(tile.Content.Type == GameTileContentType .SpawnPoint)
        {
            if(_spawnPoints.Count > 1)
            {
                _spawnPoints.Remove(tile);
                tile.Content = _contentFactory.Get(GameTileContentType.SpawnPoint);
            }
        }
        else if(tile.Content.Type == GameTileContentType.Empty)
        {
            tile.Content= _contentFactory.Get(GameTileContentType.SpawnPoint);
            _spawnPoints.Add(tile);
        }
    }

    public void ToggleWall(GameTile tile)
    {
        Debug.Log("toggleWall");
        //if (tile.Content == _contentFactory.Get(GameTileContentType.Wall))
        if (tile.Content.Type == GameTileContentType.Wall)
        {
            tile.Content = _contentFactory.Get(GameTileContentType.Empty);
            FindPaths();
        }
        else if(tile.Content.Type == GameTileContentType.Empty)
        {
            tile.Content = _contentFactory.Get(GameTileContentType.Wall);
            if(!FindPaths())
            {
                tile.Content = _contentFactory.Get(GameTileContentType.Empty);
                FindPaths();
            }
        }
    }
    
    public void ToggleTower(GameTile tile)
    {
        //Debug.Log("toggleTower");
        //if (tile.Content == _contentFactory.Get(GameTileContentType.Wall))
        if (tile.Content.Type == GameTileContentType.Tower)
        {
            _contentToUpdate.Remove(tile.Content);
            tile.Content = _contentFactory.Get(GameTileContentType.Empty);
            FindPaths();
        }
        else if(tile.Content.Type == GameTileContentType.Empty)
        {
            tile.Content = _contentFactory.Get(GameTileContentType.Tower);
            
            if(FindPaths())
            {
                _contentToUpdate.Add(tile.Content);
            }
            else
            {
                tile.Content = _contentFactory.Get(GameTileContentType.Empty);
                FindPaths();
            }
        }
        else if (tile.Content.Type == GameTileContentType.Wall)
        {
            tile.Content = _contentFactory.Get(GameTileContentType.Tower);
            _contentToUpdate.Add(tile.Content);
        }
    }

    

    public GameTile GetTile(Ray ray)
    {
        RaycastHit hit;
        Debug.DrawRay(ray.origin,ray.direction);
        if(Physics.Raycast(ray, out hit,float.MaxValue,1))
        {
            int x = (int)(hit.point.x + _size.x * 0.5f);
            int y = (int)(hit.point.y + _size.y * 0.5f);
            if(x >= 0 && x < _size.x && y >= 0 && y < _size.y)
            {
                return _tiles[((y * _size.x)+x)];
            }
        }
        return null; 
    }

    public GameTileContentType GetTileType(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            int x = (int)(hit.point.x + _size.x * 0.5f);
            int y = (int)(hit.point.y + _size.y * 0.5f);
            if (x >= 0 && x < _size.x && y >= 0 && y < _size.y)
            {
                return _tiles[((y * _size.x) + x)].Content.Type;
            }
        }
        return GameTileContentType.Empty;
    }

    public GameTile GetSpawnPoint(int index)
    {
        return _spawnPoints[index];
    }    
}
