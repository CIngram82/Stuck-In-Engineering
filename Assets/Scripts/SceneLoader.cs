using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    [SerializeField] float autoLoadNextLevelAfter;
    private void Awake()
    {
        InitSingleton(this);
    }

    private void Start()
    {
        if (autoLoadNextLevelAfter <= 0)
        {
            Debug.Log("Level auto load disabled");
        }
        else
        {
            Invoke(nameof(LoadNextLevel), autoLoadNextLevelAfter);
        }
    }

    public void LoadNextLevel()
    {
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if(nextIndex >= SceneManager.sceneCount)
        {
            LoadScene("Credits");
        }
        LoadScene(nextIndex);
    }

    public void LoadScene(int index)
    {

    }
    public void LoadScene(string name)
    {
       
    }
}
