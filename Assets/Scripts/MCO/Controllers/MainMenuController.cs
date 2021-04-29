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

    private MenuPanel newPanel;
    private MenuPanel activePanel;

    public MenuPanel ActivePanel { get => activePanel; }

    public MenuPanelEventChannelSO OnPanelChanged;


    private List<GameObject> allPanels;

    #region Unity Methods

    private void Awake()
    {
        Configure();
    }


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
        SetPanelActive(newPanel);
    }

    private void OnDestroy()
    {
        Dispose();
    }


    #endregion

    #region Public Methods

    public void SetPanelActive(int panelIndex)
    {
        Log("Activated " + panelIndex);
        switch (panelIndex)
        {
            case 0:
                newPanel = MenuPanel.Title;
                break;
            case 1:
                newPanel = MenuPanel.Main;
                break;
            case 2:
                newPanel = MenuPanel.Settings;
                break;
            case 3:
                newPanel = MenuPanel.GnomeSelection;
                break;
            case 4:
                newPanel = MenuPanel.GameOver;
                break;
        }
    }

    public void QuitGame()
    {
        GameManager.Instance.SceneController.QuitGame();
    }

    #endregion

    #region Private Methods

    private void Configure()
    {
        OnPanelChanged.OnEventRaised += SetNewPanel;
    }

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

    private void SetNewPanel(MenuPanel panel)
    {
        Log("Received OnPanelChanged");
        newPanel = panel;
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

    private void Dispose()
    {
        OnPanelChanged.OnEventRaised -= SetNewPanel;
    }

    #endregion
}
