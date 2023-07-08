using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyFactory OriginFactory { get; set; }
    private GameTile _tileFrom, _tileTo;
    private Vector3 _positionFrom, _positionTo;
    private float _progress;

    public void SpawnOn(GameTile tile)
    {
        
        transform.localPosition = tile.transform.localPosition;
        Debug.Log(transform.localPosition);
        _tileFrom = tile;
        _tileTo = tile.NextTileOnPath;
        _positionFrom = _tileFrom.transform.localPosition;
        _positionTo = _tileTo.transform.localPosition;
        _progress = 0f;
    }

    public bool GameUpdate()
    {
        _progress += Time.deltaTime;
        while(_progress >=  1)
        {
            _tileFrom = _tileTo;
            _tileTo = _tileTo.NextTileOnPath;
            if(_tileTo == null)
            {
                OriginFactory.Reclaim(this);
                return false;
            }
            _positionFrom = _positionTo;
            _positionTo = _tileTo.transform.localPosition;
            _progress -= 1f;
        }
        //transform.localPosition += Vector3.LerpUnclamped(_positionFrom, _positionTo, _progress);
        
        Vector3 direction = _tileTo.transform.localPosition - transform.position;
        transform.Translate(direction.normalized*Time.deltaTime); //*speed
        
        Debug.Log("Tile from: " + _tileFrom);
        Debug.Log("Tile to: " + _tileTo);
        Debug.Log("Position from: " + _positionFrom);
        Debug.Log("Position to:" + _positionTo);
        Debug.Log("Enemy Position: " + transform.localPosition);
        return true;
    }
}
