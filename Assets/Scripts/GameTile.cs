using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTile : MonoBehaviour
{
    [SerializeField] private Transform _arrow;
    [SerializeField] private GameTile _north, _south, _west, _east, _nextOnPath;

    [SerializeField] private int _distance;
    public bool HasPath => _distance != int.MaxValue;
    public bool IsAlternative { get; set; }

    private Quaternion _northRotation = Quaternion.Euler(0, 0, 0);
    private Quaternion _eastRotation = Quaternion.Euler(0, 0, 270);
    private Quaternion _southRotation = Quaternion.Euler(0, 0, 180);
    private Quaternion _westRotation = Quaternion.Euler(0, 0, 90);

    public GameTileContent _content;
    public GameTileContentType ContentType;

    public GameTile NextTileOnPath => _nextOnPath;

    //public Vector3 ExitPoint { get; private set; }
    public Vector3 ExitPoint;
    public GameTileContent Content
    {
        get => _content;
        set
        {
            if(_content != null)
            {
                _content.Recycle();
            }
            _content = value;
            _content.transform.localPosition = transform.localPosition;
            ContentType = _content.Type;
        }
    }
    
    public static void MakeEastWestNeighbors(GameTile east, GameTile west)
    {
        west._east = east;
        east._west = west;
    }
    public static void MakeNorthSouthNeighbors(GameTile north, GameTile south)
    {
        south._north = north;
        north._south = south;
    }

    public void ClearPath()
    {
        _distance = int.MaxValue;
        _nextOnPath = null;
    }

    public void BecomeDestination()
    {
        _distance = 0;
        _nextOnPath = null;
        ExitPoint = transform.localPosition;
    }

    private GameTile GrowPathTo(GameTile neighbor)
    {
        if (!HasPath || neighbor == null || neighbor.HasPath)
        {
            return null;
        }

        neighbor._distance = _distance + 1;
        neighbor._nextOnPath = this;
        neighbor.ExitPoint = (neighbor.transform.localPosition + transform.localPosition) * 0.5f;
        return neighbor.Content.Type !=  GameTileContentType.Wall ? neighbor : null;
        return neighbor.Content.IsBlockingPath ? null : neighbor;
    }

    public GameTile GrowPathNorth() => GrowPathTo(_north);
    public GameTile GrowPathSouth() => GrowPathTo(_south);
    public GameTile GrowPathWest() => GrowPathTo(_west);
    public GameTile GrowPathEast() => GrowPathTo(_east);

    public void ShowPath()
    {
        if (_distance == 0)
        {
            _arrow.gameObject.SetActive(false);
            return;
        }
        _arrow.gameObject.SetActive(true);
        _arrow.localRotation =
            _nextOnPath == _north ? _northRotation :
            _nextOnPath == _south ? _southRotation :
            _nextOnPath == _east ? _eastRotation :
            _westRotation;
    }
}
