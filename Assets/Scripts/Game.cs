using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private Vector2Int _boardSize;
    [SerializeField] private GameBoard _board;


    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private GameTileContentFactory _contentFactory;

    [SerializeField]
    private EnemyFactory _enemyFactory;
    [SerializeField, Range(0.1f, 10f)]
    private float _spawnSpeed;

    private float _spawnProgress = 0;

    private  EnemyCollection _enemyCollection = new EnemyCollection();
    private Ray TouchRay => _camera.ScreenPointToRay(Input.mousePosition);


    public bool LoadLevel = false; 
    private void Start()
    {
        if (LoadLevel)
        {
            _board.LoadLevel(_boardSize,_contentFactory);
        }
        else
        {
            _board.Initialize(_boardSize,_contentFactory);
        }
            
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            HandleTouch();
        }
        else if(Input.GetMouseButtonDown(1))
        {
            HandleAlternativeTouch();
        }
        _enemyCollection.GameUpdate();  
        _board.GameUpdate();
    }
    private void SpawnEnemy()
    {
        GameTile spawnPoint = _board.GetTile(TouchRay);
        //Debug.Log(spawnPoint);
        //if(spawnPoint.Content.Type == GameTileContentType.SpawnPoint)
        if(spawnPoint.ContentType == GameTileContentType.SpawnPoint)
        {
            //Debug.Log("wszystko super");
            Enemy enemy = _enemyFactory.Get();
            enemy.SpawnOn(spawnPoint);
            _enemyCollection.Add(enemy);
        }
        //enemies.GameUpdate();
        //_board.GameUpdate();
    }
    private void HandleTouch()
    {
        GameTile tile = _board.GetTile(TouchRay);
        if (tile != null)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _board.ToggleWall(tile);
            }
            else if (Input.GetKey(KeyCode.LeftControl))
            {
                _board.ToggleTower(tile);
            }
            else
            {
                SpawnEnemy();
            }
        }
    }
    private void HandleAlternativeTouch()
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
    }
    
    
}
