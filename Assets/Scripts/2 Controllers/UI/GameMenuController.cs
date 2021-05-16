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

        public Scoreboard totalScoreboard;
        public Scoreboard requiredScoreboard;

        private List<GameObject> allPanels;

        private InGameUIMode activePanel;
        private InGameUIMode nextPanel;

        public VoidEventChannelSO OnLevelLoseEvent;
        public VoidEventChannelSO OnLevelWinEvent;

        #region Unity Methods

        private void Awake()
        {
            Configure();
        }

        private void Start()
        {
            allPanels = new List<GameObject>
        {
            hud,
            pauseMenu,
            settingsMenu,
            tutorialMenu,
            gameOverMenu
        };
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

            totalScoreboard.UpdateUI(GameManager.Instance.LevelManager.lastTotalScore);
            requiredScoreboard.UpdateUI(GameManager.Instance.LevelManager.lastRequiredScore);
        }

        public void RestartLevel()
        {
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

            DeactivateAllPanels();
            switch (panel)
            {
                case InGameUIMode.HUD:
                    hud.SetActive(true);
                    break;
                case InGameUIMode.PauseMenu:
                    pauseMenu.SetActive(true);
                    break;
                case InGameUIMode.SettingsMenu:
                    settingsMenu.SetActive(true);
                    break;
                case InGameUIMode.TutorialMenu:
                    tutorialMenu.SetActive(true);
                    break;
                case InGameUIMode.GameOverMenu:
                    gameOverMenu.SetActive(true);
                    break;
            }

            activePanel = panel;
            Debug.Log("Set new panel active.");
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

