using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GnomeGardeners
{
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

        private VoidEventChannelSO OnSceneLoaded;

        public SceneState CurrentSceneState => currentScene;


        #region Unity Methods

        private void Awake()
        {
            if (GameManager.Instance.SceneController == null)
            {
                GameManager.Instance.SceneController = this;
            }
            currentScene = (SceneState)SceneManager.GetActiveScene().buildIndex;
            activeMenuPanel = MenuPanel.Title;
            FindCameraForCanvas();
            OnSceneLoaded = Resources.Load<VoidEventChannelSO>("Channels/SceneLoadedEC");
            OnSceneLoaded.OnEventRaised += UpdateState;

        }


        private void Start()
        {
            currentScene = (SceneState)SceneManager.GetActiveScene().buildIndex;
            activeInGameUI = InGameUIMode.TutorialMenu;
        }

        private void OnDestroy()
        {
            Dispose();
        }

        #endregion

        #region Public Methods

        public void LoadSceneGameplay()
        {
            if (isInTransition) { return; }
            StartCoroutine(LoadSceneAsync(SceneState.Game));
        }

        public void LoadTitleMenu()
        {
            if (isInTransition) { return; }
            StartCoroutine(LoadSceneAsync(SceneState.MainMenu));
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

        public void NextLevel()
        {
            if (isInTransition) { return; }
            
            StartCoroutine(NextLevelTransition());
        }

        public void RestartLevel()
        {
            if (isInTransition) return;

            StartCoroutine(RestartLevelTransition());
        }

        public void HandleInput()
        {
            if (activeInGameUI == InGameUIMode.HUD)
                activeInGameUI = InGameUIMode.PauseMenu;
            else
                activeInGameUI = InGameUIMode.HUD;
        }

        #endregion

        #region Private Methods

        private void FindCameraForCanvas()
        {
            if (canvas.worldCamera != null) return;
            var cameraGO = GameObject.FindGameObjectWithTag("MainCamera");
            if (cameraGO != null)
            {
                canvas.worldCamera = cameraGO.GetComponent<Camera>();
            }
        }

        private void UpdateState()
        {
            DebugLogger.Log(this, "Scene loaded, updating state.");
            currentScene = (SceneState)SceneManager.GetActiveScene().buildIndex;
        }

        private IEnumerator LoadSceneAsync(SceneState index)
        {
            isInTransition = true;

            transition.SetTrigger("FadeIn");

            yield return new WaitForSeconds(transitionTime);

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync((int)index, LoadSceneMode.Single);
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            if (index == SceneState.Game)
            {
                activeInGameUI = InGameUIMode.TutorialMenu;
                yield return StartCoroutine(GameManager.Instance.LevelManager.LoadTutorial());
            }


            isInTransition = false;
            OnSceneLoaded.RaiseEvent();
            transition.SetTrigger("FadeOut");
        }

        private IEnumerator NextLevelTransition()
        {
            isInTransition = true;

            transition.SetTrigger("FadeIn");

            yield return new WaitForSeconds(transitionTime);

            activeInGameUI = InGameUIMode.TutorialMenu;

            yield return StartCoroutine(GameManager.Instance.LevelManager.NextLevel());

            isInTransition = false;
            transition.SetTrigger("FadeOut");
        }
        
        private IEnumerator RestartLevelTransition()
        {
            isInTransition = true;

            transition.SetTrigger("FadeIn");

            yield return new WaitForSeconds(transitionTime);

            activeInGameUI = InGameUIMode.TutorialMenu;

            yield return StartCoroutine(GameManager.Instance.LevelManager.RestartLevel());
            
            isInTransition = false;
            transition.SetTrigger("FadeOut");
        }

        private void Dispose()
        {
            OnSceneLoaded.OnEventRaised -= UpdateState;
        }

        #endregion
    }
}
