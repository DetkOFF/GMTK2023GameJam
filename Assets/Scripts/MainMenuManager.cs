using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void LoadGame()
    {
        SceneManager.LoadScene("level1");
    }
    public void LoadLVL2()
    {
        SceneManager.LoadScene("level2");
    }
    public void LoadLVL3()
    {
        SceneManager.LoadScene("level3");
    }
    public void LoadLVL4()
    {
        SceneManager.LoadScene("level4");
    }
    public void LoadLVL5()
    {
        SceneManager.LoadScene("level5");
    }
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
