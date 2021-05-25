using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GnomeGardeners
{
    public class GameMenuController : MonoBehaviour
    {
        [Header("Menus")]
        public GameObject hud;
        public GameObject pauseMenu;
        public GameObject settingsMenu;
        private GameObject tutorialMenu;
        public GameObject gameOverMenu;
        private List<GameObject> allPanels;

        [Header("First selected items")] 
        public GameObject selectablePauseMenu;
        public GameObject selectableSettingsMenu;
        public GameObject selectableGameOverMenu;
        
        [Header("Other")]
        public GameObject nextLevelButton;

        private InGameUIMode activePanel;
        private InGameUIMode nextPanel;

        private VoidEventChannelSO OnLevelStartEvent;
        private VoidEventChannelSO OnLevelLoseEvent;
        private VoidEventChannelSO OnLevelWinEvent;
        
        private EventSystem eventSystem;

        #region Unity Methods

        private void Awake()
        {
            OnLevelStartEvent = Resources.Load<VoidEventChannelSO>("Channels/LevelStartEC");
            OnLevelLoseEvent = Resources.Load<VoidEventChannelSO>("Channels/LevelLoseEC");
            OnLevelWinEvent = Resources.Load<VoidEventChannelSO>("Channels/LevelWinEC");
            OnLevelStartEvent.OnEventRaised += UpdateTutorialMenu;
            OnLevelLoseEvent.OnEventRaised += SetGameOverMenuActive;
            OnLevelWinEvent.OnEventRaised += SetGameOverMenuActive;
            eventSystem = FindObjectOfType<EventSystem>();
        }

        private void Start()
        {
            allPanels = new List<GameObject>
        {
            pauseMenu,
            settingsMenu,
            gameOverMenu
        };
            GameManager.Instance.Time.PauseTime();
        }

        private void Update()
        {
            UpdateNextPanel();

            SetPanelActive(nextPanel);
        }

        private void OnDestroy()
        {
            OnLevelLoseEvent.OnEventRaised -= SetGameOverMenuActive;
            OnLevelWinEvent.OnEventRaised -= SetGameOverMenuActive;
        }

        #endregion

        #region Public Methods
        public void SetPauseMenuActive()
        {
            GameManager.Instance.SceneController.ActiveInGameUI = InGameUIMode.PauseMenu;
        }

        public void SetHUDActive()
        {
            GameManager.Instance.SceneController.ActiveInGameUI = InGameUIMode.HUD;
        }

        public void SetTutorialMenuActive()
        {
            GameManager.Instance.SceneController.ActiveInGameUI = InGameUIMode.TutorialMenu;
        }

        public void SetSettingsMenuActive()
        {
            GameManager.Instance.SceneController.ActiveInGameUI = InGameUIMode.SettingsMenu;
        }

        public void SetGameOverMenuActive()
        {
            GameManager.Instance.SceneController.ActiveInGameUI = InGameUIMode.GameOverMenu;
            if(GameManager.Instance.LevelManager.isLastLevelCompleted)
                nextLevelButton.SetActive(false);
            else
                nextLevelButton.SetActive(true);
        }

        public void NextLevel()
        {
            GameManager.Instance.SceneController.NextLevel();
        }

        public void RestartLevel()
        {
            GameManager.Instance.SceneController.RestartLevel();
        }

        public void QuitToMainMenu()
        {
            GameManager.Instance.SceneController.LoadTitleMenu();
        }

        #endregion

        #region Private Methods

        private void SetPanelActive(InGameUIMode panel)
        {
            if (panel == activePanel) { return; }

            hud.GetComponent<CanvasGroup>().alpha = 0f;
            DeactivateAllPanels();
            switch (panel)
            {
                case InGameUIMode.HUD:
                    hud.GetComponent<CanvasGroup>().alpha = 1f;
                    GameManager.Instance.Time.ResumeTime();
                    break;
                case InGameUIMode.PauseMenu:
                    pauseMenu.SetActive(true);
                    GameManager.Instance.Time.PauseTime();                    
                    eventSystem.SetSelectedGameObject(selectablePauseMenu);
                    break;
                case InGameUIMode.SettingsMenu:
                    settingsMenu.SetActive(true);
                    GameManager.Instance.Time.PauseTime();
                    eventSystem.SetSelectedGameObject(selectableSettingsMenu);
                    break;
                case InGameUIMode.TutorialMenu:
                    tutorialMenu.SetActive(true);
                    GameManager.Instance.Time.PauseTime();
                    break;
                case InGameUIMode.GameOverMenu:
                    gameOverMenu.SetActive(true);
                    GameManager.Instance.Time.PauseTime();
                    eventSystem.SetSelectedGameObject(selectableGameOverMenu);
                    break;
            }

            activePanel = panel;
            DebugLogger.Log(this, "Set new panel active.");
        }

        private void DeactivateAllPanels()
        {
            foreach (GameObject panel in allPanels)
            {
                panel.SetActive(false);
            }
        }

        private void UpdateNextPanel()
        {
            nextPanel = GameManager.Instance.SceneController.ActiveInGameUI;
        }

        private void UpdateTutorialMenu()
        {
            if (tutorialMenu)
            {
                allPanels.Remove(tutorialMenu);
                Destroy(tutorialMenu);
            }
            var tutorialMenuPrefab = GameManager.Instance.LevelManager.GetTutorialMenu();
            var canvas = gameObject.GetComponentInChildren<Canvas>();
            if(!canvas)
                Debug.LogException(new Exception(), this);
            tutorialMenu = Instantiate(tutorialMenuPrefab, canvas.transform);
            allPanels.Add(tutorialMenu);
        }

        #endregion
    }
}

