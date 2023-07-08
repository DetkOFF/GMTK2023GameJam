using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

[CreateAssetMenu]
public class GameTileContentFactory : ScriptableObject
{
    [SerializeField]
    private GameTileContent _destinationPrefab;
    [SerializeField]
    private GameTileContent _emptyPrefab;
    [SerializeField]
    private GameTileContent _spawnPointPrefab;
    [SerializeField]
    private GameTileContent _wallPrefab;
    public void Reclaim(GameTileContent content)
    {
        Destroy(content.gameObject);
    }

    public GameTileContent Get(GameTileContentType type)
    {
        switch(type)
        {
            case GameTileContentType.Destination:
                return Get(_destinationPrefab);
            case GameTileContentType.SpawnPoint:
                return Get(_spawnPointPrefab);
            case GameTileContentType.Empty:
                return Get(_emptyPrefab);
            case GameTileContentType.Wall:
                return Get(_wallPrefab);
        }

        return null;
    }

    private GameTileContent Get(GameTileContent prefab)
    {
        GameTileContent instance = Instantiate(prefab);
        instance.OriginFactory = this;
        MoveToFactoryScene(instance.gameObject);
        return instance;
    }

    private Scene _contextScene;

    private void MoveToFactoryScene(GameObject o)
    {
        if(!_contextScene.isLoaded)
        {
            if(Application.isEditor)
            {
                _contextScene = SceneManager.GetSceneByName(name);
                if(!_contextScene.isLoaded)
                {
                    _contextScene = SceneManager.CreateScene(name);
                }
            }
            else
            {
                _contextScene = SceneManager.CreateScene(name);
            }
        }
        SceneManager.MoveGameObjectToScene(o, _contextScene);
    }
}
