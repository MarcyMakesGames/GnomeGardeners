using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private bool debug = true;

    private SceneState currentScene;
    private MenuPanel activeMenuPanel;
    private InGameUIMode activeInGameUI;

    public MenuPanel ActiveMenuPanel { get => activeMenuPanel; set => activeMenuPanel = value; }
    public InGameUIMode ActiveInGameUI { get => activeInGameUI; set => activeInGameUI = value; }

    // Event Dispatcher
    public VoidEventChannelSO OnSceneLoaded;
    // Event Receiver
    public VoidEventChannelSO OnLevelLoseEvent;
    public VoidEventChannelSO OnLevelWinEvent;

    public SceneState CurrentSceneState => currentScene;


    #region Unity Methods

    private void Awake()
    {
        Configure();
    }


    private void Start()
    {
        currentScene = (SceneState) SceneManager.GetActiveScene().buildIndex;
    }

    private void OnDestroy()
    {
        Dispose();
    }

    #endregion

    #region Public Methods

    public void LoadNextScene()
    {
        StartCoroutine(LoadSceneAsync((int)currentScene + 1));
    }

    public void LoadPreviousScene()
    {
        StartCoroutine(LoadSceneAsync((int)currentScene - 1));
    }

    public void LoadSceneByString(string sceneName)
    {
        if (SceneManager.GetSceneByName(sceneName) != null)
            StartCoroutine(LoadSceneAsync(sceneName));
    }

    public void LoadSceneGameplay()
    {
        
        if (GameManager.Instance.loadTestingScenes)
        {
            StartCoroutine(LoadSceneAsync(SceneState.TestingGame));
        }
        else
        {
            StartCoroutine(LoadSceneAsync(SceneState.Game));
        }
    }

    public void LoadTitleMenu()
    {
        if (GameManager.Instance.loadTestingScenes)
        {
            StartCoroutine(LoadSceneAsync(SceneState.TestingMainMenu));
        }
        else
        {
            StartCoroutine(LoadSceneAsync(SceneState.MainMenu));
        }

        activeMenuPanel = MenuPanel.Title;
    }

    public void LoadGameOverMenu()
    {
        if (GameManager.Instance.loadTestingScenes)
        {
            StartCoroutine(LoadSceneAsync(SceneState.TestingMainMenu));
        }
        else
        {
            StartCoroutine(LoadSceneAsync(SceneState.MainMenu));
        }
        activeMenuPanel = MenuPanel.GameOver;
    }

    public void QuitGame()
    {
        //We should save any player prefs before this point.
#if UNITY_STANDALONE
        Application.Quit();
#endif
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void RestartLevel()
    {
        var scene = SceneManager.GetActiveScene();
        SceneManager.UnloadSceneAsync(scene);

        if (GameManager.Instance.loadTestingScenes)
        {
            StartCoroutine(LoadSceneAsync(SceneState.TestingGame));
        }
        else
        {
            StartCoroutine(LoadSceneAsync(SceneState.Game));
        }
    }

    #endregion

    #region Private Methods

    private void Configure()
    {
        if(GameManager.Instance.SceneController == null)
        {
            GameManager.Instance.SceneController = this;
            OnLevelLoseEvent.OnEventRaised += LoadGameOverMenu;
            OnLevelWinEvent.OnEventRaised += LoadGameOverMenu;
            OnSceneLoaded.OnEventRaised += UpdateState;
        }
        currentScene = (SceneState)SceneManager.GetActiveScene().buildIndex;
        activeMenuPanel = MenuPanel.Title;
    }

    private void UpdateState()
    {
        Log("Scene loaded, updating state.");
        currentScene = (SceneState) SceneManager.GetActiveScene().buildIndex;
    }

    private IEnumerator LoadSceneAsync(int index)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        OnSceneLoaded.RaiseEvent();
        Log("Scene Loaded");
    }

    private IEnumerator LoadSceneAsync(SceneState index)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync((int)index);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        OnSceneLoaded.RaiseEvent();
        Log("Scene Loaded");
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        OnSceneLoaded.RaiseEvent();
        Log("Scene Loaded");
    }

    private void Dispose()
    {
        OnLevelLoseEvent.OnEventRaised -= LoadGameOverMenu;
        OnLevelWinEvent.OnEventRaised -= LoadGameOverMenu;
        OnSceneLoaded.OnEventRaised -= UpdateState;
    }

    private void Log(string msg)
    {
        if (!debug) { return; }
        Debug.Log("[SceneController]: " + msg);
    }

    #endregion
}
