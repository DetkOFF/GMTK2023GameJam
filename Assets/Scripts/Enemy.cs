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
    private float _health;
    private float _speed;
    private float _price;
    public Action OnEnemyPassed;

    public void Initialize(float health, float speed, float price)
    {
        _health = health;
        _speed = speed;
        _price = price;
    }
    public void ApplyDamage(float damage)
    {
        Debug.Assert(damage >= 0f, "Negative damage applied.");
        _health -= damage;
    }

    public void SpawnOn(GameTile tile)
    {
        
        transform.localPosition = tile.transform.localPosition;
        Debug.Log(transform.localPosition);
        _tileFrom = tile;
        _tileTo = tile.NextTileOnPath;
        _positionFrom = _tileFrom.transform.localPosition;
        _positionTo = _tileTo.ExitPoint;
        _progress = 0f;
    }

    public bool GameUpdate()
    {
        if(_health <= 0f)
        {
            OriginFactory.Reclaim(this);
            return false;
        }
        _progress += Time.deltaTime * _speed;
        while(_progress >=  1)
        {
            _tileFrom = _tileTo;
            _tileTo = _tileTo.NextTileOnPath;
            if(_tileTo == null)
            {
                OnEnemyPassed?.Invoke();
                OriginFactory.Reclaim(this);
                return false;
            }
            _positionFrom = _positionTo;
            _positionTo = _tileTo.ExitPoint;
            _progress -= 1f;
        }
        transform.localPosition = Vector3.LerpUnclamped(_positionFrom, _positionTo, _progress);
        
        //Vector3 direction = _tileTo.ExitPoint - transform.position;
        //transform.Translate(direction.normalized*Time.deltaTime*_speed); //*speed
        return true;
    }
    public float GetEnemyPrice()
    {
        return _price;
    }
    
}
