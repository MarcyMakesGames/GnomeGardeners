using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUIManager : MonoBehaviour
{
    public GameObject hud;
    public GameObject pauseMenu;
    public GameObject settingsMenu;
    public GameObject tutorialMenu;

    private List<GameObject> allPanels;

    private InGameUIMode activePanel;
    private InGameUIMode nextPanel;

    #region Unity Methods

    private void Start()
    {
        allPanels = new List<GameObject>();
        allPanels.Add(hud);
        allPanels.Add(pauseMenu);
        allPanels.Add(settingsMenu);
        allPanels.Add(tutorialMenu);
    }

    private void Update()
    {
        UpdateNextPanel();

        SetPanelActive(nextPanel);
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

    #endregion
}
