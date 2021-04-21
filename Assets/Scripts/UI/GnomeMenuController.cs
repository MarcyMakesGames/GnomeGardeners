using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GnomeMenuController : MonoBehaviour
{
    private int PlayerIndex;

    [SerializeField]
    private TextMeshProUGUI titleText;
    [SerializeField]
    private GameObject readyPanel;
    [SerializeField]
    private GameObject startPanel;
    [SerializeField]
    private Button startButton;

    private float inputDelayTime = 1.5f;
    private bool inputEnabled;

    public void SetPlayerIndex(int index)
    {
        PlayerIndex = index;
        titleText.SetText("Player " + (PlayerIndex + 1).ToString());

        inputDelayTime = Time.time + inputDelayTime;
    }

    //public void SetGnomeSkin(GnomeSkin skin)
    //{
    //    if (!inputEnabled)
    //        return;

    //    Here we would assign the player skin to the PlayerConfig
    //    return;
    //}

    public void SetPlayerReady()
    {
        if (!inputEnabled)
            return;

        GameManager.Instance.PlayerConfigManager.ReadyPlayer(PlayerIndex);

        startButton.gameObject.SetActive(true);
        readyPanel.gameObject.SetActive(false);


    }

    public void StartGame()
    {
        GameManager.Instance.PlayerConfigManager.StartGameCheck();
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (Time.time > inputDelayTime)
            inputEnabled = true;
    }


}
