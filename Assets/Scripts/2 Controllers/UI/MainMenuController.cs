using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
    public class MainMenuController : MonoBehaviour
    {
        private readonly bool debug = false;

        public GameObject titlePanel;
        public GameObject mainPanel;
        public GameObject settingsPanel;
        public GameObject gnomeSelectionPanel;
        public GameObject gameOverPanel;

        private Animator transition;
        public float transitionTime = 1f;

        private MenuPanel nextPanel;
        private MenuPanel activePanel;

        public MenuPanel ActivePanel { get => activePanel; }


        private List<GameObject> allPanels;

        #region Unity Methods

        private void Start()
        {
            allPanels = new List<GameObject>
        {
            titlePanel,
            mainPanel,
            settingsPanel,
            gnomeSelectionPanel,
            gameOverPanel
        };
            transition = GameManager.Instance.SceneController.Transition;
        }

        private void Update()
        {
            UpdateNextPanel();

            SetPanelActive(nextPanel);
        }


        #endregion

        #region Public Methods

        public void SetPanelActive(int panelIndex)
        {
            DebugLogger.Log(this, "Activated " + panelIndex);
            switch (panelIndex)
            {
                case 0:
                    GameManager.Instance.SceneController.ActiveMenuPanel = MenuPanel.Title;
                    nextPanel = MenuPanel.Title;
                    break;
                case 1:
                    GameManager.Instance.SceneController.ActiveMenuPanel = MenuPanel.Main;
                    nextPanel = MenuPanel.Main;
                    break;
                case 2:
                    GameManager.Instance.SceneController.ActiveMenuPanel = MenuPanel.Settings;
                    nextPanel = MenuPanel.Settings;
                    break;
                case 3:
                    GameManager.Instance.SceneController.ActiveMenuPanel = MenuPanel.GnomeSelection;
                    nextPanel = MenuPanel.GnomeSelection;
                    GameManager.Instance.PlayerConfigManager.canJoinPlayers = true;
                    break;
            }
        }

        public void QuitGame()
        {
            GameManager.Instance.SceneController.QuitGame();
        }

        #endregion

        #region Private Methods

        private void SetPanelActive(MenuPanel panel)
        {
            if (panel == ActivePanel) { return; }

            switch (panel)
            {
                case MenuPanel.Title:
                    StartCoroutine(TransitionIntoPanel(titlePanel));

                    break;
                case MenuPanel.Main:
                    StartCoroutine(TransitionIntoPanel(mainPanel));

                    break;
                case MenuPanel.Settings:
                    StartCoroutine(TransitionIntoPanel(settingsPanel));

                    break;
                case MenuPanel.GnomeSelection:
                    StartCoroutine(TransitionIntoPanel(gnomeSelectionPanel));

                    break;
            }

            activePanel = panel;
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
            nextPanel = GameManager.Instance.SceneController.ActiveMenuPanel;
        }

        private IEnumerator TransitionIntoPanel(GameObject panel)
        {
            transition.SetTrigger("FadeIn");

            yield return new WaitForSeconds(transitionTime);

            DeactivateAllPanels();

            panel.SetActive(true);

            transition.SetTrigger("FadeOut");

            yield break;
        }

        #endregion
    }
}

