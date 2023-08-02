using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private EnemyFactory enemyFactory;

    [SerializeField] private EnemyChooser enemyChooser;
    
    
    private  EnemyCollection enemyCollection = new EnemyCollection();
    public List<GameTileContent> contentToUpdate = new List<GameTileContent>();

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleTouch();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
            enemyChooser.ChangeCurrentEnemy(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            enemyChooser.ChangeCurrentEnemy(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            enemyChooser.ChangeCurrentEnemy(2);
        
        
        UpdateGameModules();

    }

    private void UpdateGameModules()
    {
        //enemies update (moving)
        enemyCollection.GameUpdate();
        
        //towers++ update (finding target & shooting)
        for (int i = 0; i < contentToUpdate.Count; i++)
        {
            contentToUpdate[i].GameUpdate();
        }
    }

    private RoadTile GetRoadTile()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if(hit)
        {
            return hit.collider.GetComponent<RoadTile>();
        }
        return null; 
    }
    
    private void SpawnEnemy()
    {
        Debug.Log("spawn enemy");
        RoadTile spawnPoint = GetRoadTile();
        Debug.Log(spawnPoint);
        if(spawnPoint != null  && spawnPoint.RoadType == RoadTileType.SpawnPoint)
        {
            /*if(_cashManager.GetBalance() >= 0)
            {
                Enemy enemy = _enemyFactory.Get((EnemyType)Random.Range(0, 3));
                _cashManager.RemoveFromBalance(enemy.GetEnemyPrice());
                //enemy.SpawnOn(spawnPoint);
                _enemyCollection.Add(enemy);
            }
            else
            {
                Debug.Log("Game failed");
            }*/
            Enemy enemy = enemyFactory.Get((EnemyType)enemyChooser.GetPickedEnemy());
            enemy.SpawnOn(spawnPoint);
            enemyCollection.Add(enemy);
        }
    }
    
    private void HandleTouch()
    {
        SpawnEnemy();
        
    }
    /*private void HandleAlternativeTouch()
    {
        GameTile tile = _board.GetTile(TouchRay);
        if (tile != null)
        {
            if(Input.GetKey(KeyCode.LeftShift))
            {
                _board.ToggleDestination(tile);
            }
            else
            {
                _board.ToggleSpawnPoint(tile);
            }
        }
    }*/
}
