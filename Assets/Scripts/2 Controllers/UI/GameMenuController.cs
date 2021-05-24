using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GnomeGardeners
{
    public class GameMenuController : MonoBehaviour
    {
        public GameObject hud;
        public GameObject pauseMenu;
        public GameObject settingsMenu;
        public GameObject tutorialMenu;
        public GameObject gameOverMenu;
        private List<GameObject> allPanels;

        private EventSystem eventSystem;
        public GameObject quitButton;

        private InGameUIMode activePanel;
        private InGameUIMode nextPanel;

        public VoidEventChannelSO OnLevelLoseEvent;
        public VoidEventChannelSO OnLevelWinEvent;

        #region Unity Methods

        private void Awake()
        {
            Configure();
            eventSystem = FindObjectOfType<EventSystem>();
        }

        private void Start()
        {
            allPanels = new List<GameObject>
        {
            pauseMenu,
            settingsMenu,
            tutorialMenu,
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
            Dispose();
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
        }

        public void RestartLevel()
        {
            GameManager.Instance.SceneController.ActiveInGameUI = InGameUIMode.TutorialMenu;
            GameManager.Instance.SceneController.LoadSceneGameplay();
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
                    break;
                case InGameUIMode.SettingsMenu:
                    settingsMenu.SetActive(true);
                    GameManager.Instance.Time.PauseTime();
                    break;
                case InGameUIMode.TutorialMenu:
                    tutorialMenu.SetActive(true);
                    GameManager.Instance.Time.PauseTime();
                    break;
                case InGameUIMode.GameOverMenu:
                    gameOverMenu.SetActive(true);
                    GameManager.Instance.Time.PauseTime();
                    eventSystem.SetSelectedGameObject(quitButton);
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

        private void Configure()
        {
            OnLevelLoseEvent.OnEventRaised += SetGameOverMenuActive;
            OnLevelWinEvent.OnEventRaised += SetGameOverMenuActive;
        }

        private void Dispose()
        {
            OnLevelLoseEvent.OnEventRaised -= SetGameOverMenuActive;
            OnLevelWinEvent.OnEventRaised -= SetGameOverMenuActive;
        }

        #endregion
    }
}

