using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    private bool debug = true;

    public GameObject titlePanel;
    public GameObject mainPanel;
    public GameObject settingsPanel;
    public GameObject gnomeSelectionPanel;
    public GameObject gameOverPanel;

    public Scoreboard totalScoreboard;
    public Scoreboard requiredScoreboard;

    private MenuPanel nextPanel;
    private MenuPanel activePanel;

    public MenuPanel ActivePanel { get => activePanel; }


    private List<GameObject> allPanels;

    #region Unity Methods

    private void Start()
    {
        allPanels = new List<GameObject>();
        allPanels.Add(titlePanel);
        allPanels.Add(mainPanel);
        allPanels.Add(settingsPanel);
        allPanels.Add(gnomeSelectionPanel);
        allPanels.Add(gameOverPanel);
    }

    private void Update()
    {
        UpdateNextPanel();

        SetPanelActive(nextPanel);
    }

    private void UpdateNextPanel()
    {
        nextPanel = GameManager.Instance.SceneController.ActivePanel;
    }


    #endregion

    #region Public Methods

    public void SetPanelActive(int panelIndex)
    {
        Log("Activated " + panelIndex);
        switch (panelIndex)
        {
            case 0:
                GameManager.Instance.SceneController.ActivePanel = MenuPanel.Title;
                nextPanel = MenuPanel.Title;
                break;
            case 1:
                GameManager.Instance.SceneController.ActivePanel = MenuPanel.Main;
                nextPanel = MenuPanel.Main;
                break;
            case 2:
                GameManager.Instance.SceneController.ActivePanel = MenuPanel.Settings;
                nextPanel = MenuPanel.Settings;
                break;
            case 3:
                GameManager.Instance.SceneController.ActivePanel = MenuPanel.GnomeSelection;
                nextPanel = MenuPanel.GnomeSelection;
                break;
            case 4:
                GameManager.Instance.SceneController.ActivePanel = MenuPanel.GameOver;
                nextPanel = MenuPanel.GameOver;
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
        if(panel == ActivePanel) { return; }

        DeactivateAllPanels();
        switch (panel)
        {
            case MenuPanel.Title:
                titlePanel.SetActive(true);

                break;
            case MenuPanel.Main:
                mainPanel.SetActive(true);

                break;
            case MenuPanel.Settings:
                settingsPanel.SetActive(true);

                break;
            case MenuPanel.GnomeSelection:
                gnomeSelectionPanel.SetActive(true);

                break;
            case MenuPanel.GameOver:
                gameOverPanel.SetActive(true);
                totalScoreboard.UpdateUI(GameManager.Instance.LevelManager.lastTotalScore);
                requiredScoreboard.UpdateUI(GameManager.Instance.LevelManager.lastRequiredScore);
                break;
        }

        activePanel = panel;
    }

    private void DeactivateAllPanels()
    {
        foreach(GameObject panel in allPanels)
        {
            panel.SetActive(false);
        }
    }

    private void SetNextPanel(MenuPanel panel)
    {
        nextPanel = panel;
    }

    private void Log(string msg)
    {
        if (!debug) { return; }
        Debug.Log("[MainMenuController]: " + msg);
    }

    private void LogWarning(string msg)
    {
        if (!debug) { return; }
        Debug.LogWarning("[MainMenuController]: " + msg);
    }
    #endregion
}
