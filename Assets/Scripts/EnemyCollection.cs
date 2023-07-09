using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyCollection
{
    private List<Enemy> _enemies = new List<Enemy>();
    public int _passedAmmount { get; private set; } = 0;
    public void Add(Enemy enemy)
    {
        _enemies.Add(enemy);
    }

    public void GameUpdate()
    {
        for(int i = 0; i < _enemies.Count; i++)
        {
            if(!_enemies[i].GameUpdate())
            {
                int lastIndex = _enemies.Count - 1;
                _enemies[i] = _enemies[lastIndex];
                EnemyPassed(i);
                _enemies.RemoveAt(lastIndex);
                i -= 1;
            }
        }
        DisplayPassedEnemies();
    }
    private void EnemyPassed(int index)
    {
        if(_enemies[index]._passed)
        {
            _passedAmmount++;
        }
    }
    private void DisplayPassedEnemies()
    {
        Debug.Log(_passedAmmount);
    }

   
    
}
