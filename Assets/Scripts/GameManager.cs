using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _gameManager;
    private static Scene scene;
    private void Start()
    {
        scene = SceneManager.GetActiveScene();
        if (scene.buildIndex != 0)
        {
            
        
            if (_gameManager != null && _gameManager != this)
            {
                Destroy(gameObject);
                return;
            }
            _gameManager = this;
            DontDestroyOnLoad(this);
        }
    }
    
    public void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
