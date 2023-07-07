using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GameTile : MonoBehaviour
{
    [SerializeField] private Transform _arrow;
    private GameTile _north, _south, _west, _east, _nextOnPath;

    private int _distance;
    public bool HasPath => _distance != int.MaxValue;
    public bool IsAlternative { get; set; }
    
    private Quaternion _northRotation = Quaternion.Euler(0,0,0);
    private Quaternion _eastRotation = Quaternion.Euler(0,0,270);
    private Quaternion _southRotation = Quaternion.Euler(0,0,180);
    private Quaternion _westRotation = Quaternion.Euler(0,0,90);
    
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
    }

    private GameTile GrowPathTo(GameTile neighbor)
    {
        if (!HasPath || neighbor == null || neighbor.HasPath)
        {
            return null;
        }

        neighbor._distance = _distance + 1;
        neighbor._nextOnPath = this;
        return neighbor;
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
