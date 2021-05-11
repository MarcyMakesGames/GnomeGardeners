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
    private bool isInTransition = false;

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
        if (isInTransition) { return; }

        StartCoroutine(LoadSceneAsync((int)currentScene + 1));
    }

    public void LoadPreviousScene()
    {
        if (isInTransition) { return; }

        StartCoroutine(LoadSceneAsync((int)currentScene - 1));
    }

    public void LoadSceneByString(string sceneName)
    {
        if (isInTransition) { return; }

        if (SceneManager.GetSceneByName(sceneName) != null)
            StartCoroutine(LoadSceneAsync(sceneName));
    }

    public void LoadSceneGameplay()
    {
        if (isInTransition) { return; }
        
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
        if (isInTransition) { return; }

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
        if (isInTransition) { return; }

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
            var cameraGO = GameObject.FindGameObjectWithTag("MainCamera");
            if(cameraGO != null)
            {
                canvas.worldCamera = cameraGO.GetComponent<Camera>();
            }
        }
    }

    private void UpdateState()
    {
        Log("Scene loaded, updating state.");
        currentScene = (SceneState) SceneManager.GetActiveScene().buildIndex;
    }

    private IEnumerator LoadSceneAsync(int index)
    {
        isInTransition = true;

        transition.SetTrigger("FadeIn");

        yield return new WaitForSeconds(transitionTime);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        Log("Scene Loaded");
        isInTransition = false;
        OnSceneLoaded.RaiseEvent();
        transition.SetTrigger("FadeOut");
    }

    private IEnumerator LoadSceneAsync(SceneState index)
    {
        isInTransition = true;

        transition.SetTrigger("FadeIn");

        yield return new WaitForSeconds(transitionTime);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync((int)index);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }


        Log("Scene Loaded");
        isInTransition = false;
        OnSceneLoaded.RaiseEvent();
        transition.SetTrigger("FadeOut");
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        isInTransition = true;
        transition.SetTrigger("FadeIn");

        yield return new WaitForSeconds(transitionTime);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        Log("Scene Loaded");
        isInTransition = false;
        OnSceneLoaded.RaiseEvent();
        transition.SetTrigger("FadeOut");
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
