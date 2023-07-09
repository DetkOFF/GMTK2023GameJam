using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu]
public class EnemyFactory : GameObjectFactory
{
    [SerializeField]
    private Enemy _prefab;
    [SerializeField]
    private float _health;
    [SerializeField]
    private float _speed;

    public Enemy Get()
    {
        Enemy instance = CreateGameObjectInstance(_prefab);
        instance.OriginFactory = this;
        instance.Initialize(_health, _speed);
        return instance;
    }

    public void Reclaim(Enemy enemy)
    {
        Destroy(enemy.gameObject);
    }
}
