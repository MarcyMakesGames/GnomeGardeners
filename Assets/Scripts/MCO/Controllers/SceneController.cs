using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private int currentScene;

    public int CurrentSceneIndex => currentScene;

    public void LoadNextScene()
    {
        SceneManager.LoadScene(CurrentSceneIndex + 1);
    }

    public void LoadSceneByString(string sceneName)
    {
        if(SceneManager.GetSceneByName(sceneName) != null)
            SceneManager.LoadScene(sceneName);
    }

    public void LoadTitleScene()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        //We should save any player prefs before this point.
        Application.Quit();
    }

    private void Awake()
    {
        GameManager.Instance.SceneController = this;
    }

    private void Start()
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;
    }
}
