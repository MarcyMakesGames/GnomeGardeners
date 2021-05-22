using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

namespace GnomeGardeners
{
    
    public class MainMenuController : MonoBehaviour
    {
        private readonly bool debug = false;

        public GameObject panelTitle;
        public GameObject panelMain;
        public GameObject panelSettings;
        public GameObject panelPlayerSelection;
        public GameObject panelManual;
        public GameObject panelCredits;

        public GameObject backgroundTitle;
        public GameObject backgroundCredits;

        private Animator transition;
        public float transitionTime = 1f;

        private MenuPanel nextPanel;
        private MenuPanel activePanel;

        private InputSystemUIInputModule uiInputModule;
        private EventSystem eventSystem;

        public MenuPanel ActivePanel { get => activePanel; }


        private List<GameObject> allPanels;
        private List<GameObject> allBackgrounds;

        #region Unity Methods

        private void Awake()
        {
            uiInputModule = GetComponent<InputSystemUIInputModule>();
            eventSystem = GetComponent<EventSystem>();
        }

        private void Start()
        {
            allPanels = new List<GameObject>
            {
                panelTitle,
                panelMain,
                panelSettings,
                panelPlayerSelection,
                panelCredits,
                panelManual
            };

            allBackgrounds = new List<GameObject>
            {
                backgroundTitle,
                backgroundCredits,
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
                case 4:
                    GameManager.Instance.SceneController.ActiveMenuPanel = MenuPanel.Credits;
                    nextPanel = MenuPanel.Credits;
                    break;
                case 5:
                    GameManager.Instance.SceneController.ActiveMenuPanel = MenuPanel.Manual;
                    nextPanel = MenuPanel.Manual;
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
                    StartCoroutine(TransitionIntoPanel(panelTitle, backgroundTitle));

                    break;
                case MenuPanel.Main:
                    StartCoroutine(TransitionIntoPanel(panelMain));

                    break;
                case MenuPanel.Settings:
                    StartCoroutine(TransitionIntoPanel(panelSettings));

                    break;
                case MenuPanel.GnomeSelection:
                    StartCoroutine(TransitionIntoPanel(panelPlayerSelection));

                    break;
                case MenuPanel.Credits:
                    StartCoroutine(TransitionIntoPanel(panelCredits, backgroundCredits));
                    break;
                case MenuPanel.Manual:
                    StartCoroutine(TransitionIntoPanel(panelManual));
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
            foreach (GameObject background in allBackgrounds)
            {
                background.SetActive(false);

            }
        }

        private void UpdateNextPanel()
        {
            nextPanel = GameManager.Instance.SceneController.ActiveMenuPanel;
        }

        private IEnumerator TransitionIntoPanel(GameObject panel, GameObject background = null)
        {
            transition.SetTrigger("FadeIn");

            yield return new WaitForSeconds(transitionTime);

            DeactivateAllPanels();

            panel.SetActive(true);
            eventSystem.SetSelectedGameObject(panel.transform.GetChild(0).gameObject);

            if (background != null)
                background.SetActive(true);

            transition.SetTrigger("FadeOut");

            yield break;
        }

        #endregion
    }
}

