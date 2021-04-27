using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    private bool debug = true;

    public GameObject titlePanel;
    public GameObject mainPanel;
    public GameObject settingsPanel;
    public GameObject gnomeSelectionPanel;
    public GameObject gameOverPanel;
    public MenuPanel ActivePanel { get; set; }

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
        SetPanelActive(ActivePanel);
    }


    #endregion

    #region Public Methods

    public void SetPanelActive(int panelIndex)
    {
        Log("Activated " + panelIndex);
        switch (panelIndex)
        {
            case 0:
                ActivePanel = MenuPanel.Title;
                break;
            case 1:
                ActivePanel = MenuPanel.Main;
                break;
            case 2:
                ActivePanel = MenuPanel.Settings;
                break;
            case 3:
                ActivePanel = MenuPanel.GnomeSelection;
                break;
            case 4:
                ActivePanel = MenuPanel.GameOver;
                break;
        }
    }

    public void QuitGame()
    {
#if UNITY_STANDALONE
        Application.Quit();
#endif
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
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

                break;
        }

        ActivePanel = panel;
    }

    private void DeactivateAllPanels()
    {
        foreach(GameObject panel in allPanels)
        {
            panel.SetActive(false);
        }
    }
    private void Log(string msg)
    {
        if (!debug) { return; }
        Debug.Log("[MainMenuManager]: " + msg);
    }

    private void LogWarning(string msg)
    {
        if (!debug) { return; }
        Debug.LogWarning("[MainMenuManager]: " + msg);
    }

    #endregion
}
