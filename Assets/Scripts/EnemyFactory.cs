using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu]
public class EnemyFactory : GameObjectFactory
{
    [Serializable]
    class EnemyConfig
    {
        public Enemy _prefab;
        public float _speed;
        public float _health;
        public float _price;
    }
    [SerializeField]
    private EnemyConfig _strong, _standard, _speedy;
    public Enemy Get(EnemyType type)
    {
        var config = GetConfig(type);
        Enemy instance = CreateGameObjectInstance(config._prefab);
        instance.OriginFactory = this;
        instance.Initialize(config._health, config._speed, config._price);
        return instance;
    }

    private EnemyConfig GetConfig(EnemyType type)
    {
        switch(type)
        {
            case EnemyType.Strong:
                return _strong;
            case EnemyType.Standard:
                return _standard;
            case EnemyType.Speedy:
                return _speedy;
        }
        Debug.LogError($"No config for {type}");
        return _standard;
    }

    public void Reclaim(Enemy enemy)
    {
        Destroy(enemy.gameObject);
    }
}
