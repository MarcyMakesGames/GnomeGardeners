using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private readonly bool debug = false;

    private SceneState currentScene;
    private MenuPanel activeMenuPanel;
    private InGameUIMode activeInGameUI;

    public Canvas canvas;
    public Animator transition;
    public float transitionTime = 1f;

    public MenuPanel ActiveMenuPanel { get => activeMenuPanel; set => activeMenuPanel = value; }
    public InGameUIMode ActiveInGameUI { get => activeInGameUI; set => activeInGameUI = value; }
    public Animator Transition { get => transition; }

    public VoidEventChannelSO OnSceneLoaded;

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
            OnSceneLoaded.OnEventRaised += UpdateState;
        }
        currentScene = (SceneState)SceneManager.GetActiveScene().buildIndex;
        activeMenuPanel = MenuPanel.Title;
        FindCameraForCanvas();
    }

    private void FindCameraForCanvas()
    {
        if (canvas.worldCamera == null)
        {
            canvas.worldCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }
    }

    private void UpdateState()
    {
        Log("Scene loaded, updating state.");
        currentScene = (SceneState) SceneManager.GetActiveScene().buildIndex;

        // note: quick fix for transition scene stuck after load. (probably due to multiple calling of coroutines)
        transition.SetTrigger("FadeOut");
    }

    private IEnumerator LoadSceneAsync(int index)
    {
        transition.SetTrigger("FadeIn");

        yield return new WaitForSeconds(transitionTime);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        transition.SetTrigger("FadeOut");

        OnSceneLoaded.RaiseEvent();
        Log("Scene Loaded");
    }

    private IEnumerator LoadSceneAsync(SceneState index)
    {
        transition.SetTrigger("FadeIn");

        yield return new WaitForSeconds(transitionTime);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync((int)index);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        transition.SetTrigger("FadeOut");

        OnSceneLoaded.RaiseEvent();
        Log("Scene Loaded");
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        transition.SetTrigger("FadeIn");

        yield return new WaitForSeconds(transitionTime);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        transition.SetTrigger("FadeOut");

        OnSceneLoaded.RaiseEvent();
        Log("Scene Loaded");
    }

    private void Dispose()
    {
        OnSceneLoaded.OnEventRaised -= UpdateState;
    }

    private void Log(string msg)
    {
        if (!debug) { return; }
        Debug.Log("[SceneController]: " + msg);
    }

    #endregion
}
