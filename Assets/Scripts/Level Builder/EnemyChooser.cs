using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChooser : MonoBehaviour
{
    private int currentEnemy;

    private void Start()
    {
        currentEnemy = 0;
    }


    public void ChangeCurrentEnemy(int newEnemy)
    {
        currentEnemy = newEnemy;
    }

    public int GetPickedEnemy()
    {
        return currentEnemy;
    }
}
