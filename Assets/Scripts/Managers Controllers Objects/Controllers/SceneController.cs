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

    public void LoadTitleScene()
    {
        SceneManager.LoadScene(0);
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
