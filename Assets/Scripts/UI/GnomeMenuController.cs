using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GnomeMenuController : MonoBehaviour
{
    public bool debugMenu;

    private int PlayerIndex;
    private GameObject canvasGroup;

    [SerializeField]
    private TextMeshProUGUI titleText;
    [SerializeField]
    private GameObject skinPanel;
    [SerializeField]
    private GameObject readyPanel;
    [SerializeField]
    private Button readyButton;
    [SerializeField]
    private TextMeshProUGUI readyText;
    [SerializeField]
    private Button startButton;
    [SerializeField]
    private Button quitButton;

    private float inputDelayTime = 1.5f;
    private bool inputEnabled;

    public void SetPlayerIndex(int index)
    {
        PlayerIndex = index;
        titleText.SetText("Player " + (PlayerIndex + 1).ToString());

        inputDelayTime = Time.time + inputDelayTime;
    }

    public void SetGnomeSkin(GnomeSkin skin)
    {
        if (!inputEnabled)
            return;

        return;
    }

    //This is purely a debug function prior to implementing gnome skins.
    public void SetGnomeSkin(int skinIndex)
    {
        if (!inputEnabled)
            return;

        readyPanel.gameObject.SetActive(true);
        skinPanel.gameObject.SetActive(false);

        readyButton.Select();

        return;
    }

    public void SetPlayerReady()
    {
        if (!inputEnabled)
            return;

        GameManager.Instance.PlayerConfigManager.ReadyPlayer(PlayerIndex);
        readyButton.gameObject.SetActive(false);
        startButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
        readyText.gameObject.SetActive(true);

        startButton.Select();
    }

    public void StartGame()
    {
        GameManager.Instance.PlayerConfigManager.StartGameCheck();
    }

    public void QuitGame()
    {
        GameManager.Instance.SceneController.QuitGame();
    }

    private void Start()
    {
        canvasGroup = transform.parent.gameObject;
        debugMenu = GameManager.Instance.DebugMenu;
    }

    private void Update()
    {
        if (Time.time > inputDelayTime)
            inputEnabled = true;
    }
}
