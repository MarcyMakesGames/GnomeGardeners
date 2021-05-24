using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GnomeGardeners
{
    public class GnomeMenuController : MonoBehaviour
    {
        private int PlayerIndex;

        [SerializeField] private GameObject skinPanel;
        [SerializeField] private Button confirmButton;
        [SerializeField] private GameObject readyPanel;
        [SerializeField] private Button readyButton;

        private float inputDelayTime = 0.5f;
        private bool inputEnabled;

        private void Update()
        {
            if (Time.time > inputDelayTime)
                inputEnabled = true;

            if (GameManager.Instance.PlayerConfigManager.AllPlayerAreReady())
                StartGame();
        }

        public void SetPlayerIndex(int index)
        {
            PlayerIndex = index;

            inputDelayTime = Time.time + inputDelayTime;
        }

        public void CycleGnomeSkin(bool right) 
        {
            if (right)
            {
                // gnome skin ++
            }
            else
            {
                // gnome skin --
            }
        }

        public void ConfirmSkin()
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

            readyButton.GetComponent<Image>().color = Color.red;
        }

        public void StartGame()
        {
            GameManager.Instance.PlayerConfigManager.StartGameCheck();
        }

        public void QuitGame()
        {
            GameManager.Instance.SceneController.QuitGame();
        }
    }
}
