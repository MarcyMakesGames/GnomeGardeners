using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.UI;

namespace GnomeGardeners
{
    public class GnomeMenuController : MonoBehaviour
    {
        [SerializeField] private Button readyButton;
        [SerializeField] private Image gnomeImage;
        [SerializeField] private Image playerIcon;
        [SerializeField] private bool Player1;
        private GnomeSelectorController gnomeSelector;
        private GnomeSkinObject gnomeSkin;
        private int playerIndex = -1;
        private bool isReady = false;
        public int PlayerIndex { get => playerIndex; set => SetPlayerIndex(value); }


        private float inputDelayTime = 0.15f;
        private bool inputEnabled;

        private void OnEnable()
        {
            gnomeSelector = FindObjectOfType<GnomeSelectorController>();
            gnomeImage.sprite = null;
            readyButton.GetComponentInChildren<TMP_Text>().color = Color.white;
            isReady = false;

            if (playerIndex == 0 && GameManager.Instance.PlayerConfigManager.PlayerCount != 0 || 
                Player1 && GameManager.Instance.PlayerConfigManager.PlayerCount != 0)
            {
                playerIndex = 0;
                GameManager.Instance.PlayerConfigManager.PlayerConfigs[0].Input.uiInputModule = GetComponent<InputSystemUIInputModule>();
                GetComponent<InputSystemUIInputModule>().enabled = true;
                gnomeImage.enabled = true;

                if (gnomeSkin == null)
                    gnomeSkin = gnomeSelector.GetNextGnome();

                gnomeImage.sprite = gnomeSkin.GnomeSprite;
            }
        }

        private void OnDisable()
        {
            GetComponent<InputSystemUIInputModule>().enabled = false;
            gnomeImage.enabled = false;
        }

        private void Update()
        {
            if (Time.time > inputDelayTime)
                inputEnabled = true;
        }

        public void CycleGnomeSkin(bool right) 
        {
            if (isReady)
                return;

            if (right)
                gnomeSkin = gnomeSelector.GetNextGnome(gnomeSkin);
            else
                gnomeSkin = gnomeSelector.GetPreviousGnome(gnomeSkin);

            playerIcon.sprite = gnomeSelector.GetPlayerIcon(gnomeSkin, PlayerIndex);
            gnomeImage.sprite = gnomeSkin.GnomeSprite;
        }

        public void SetPlayerReady()
        {
            if (!inputEnabled)
                return;

            readyButton.GetComponentInChildren<TMP_Text>().color = Color.red;
            GameManager.Instance.PlayerConfigManager.ReadyPlayer(PlayerIndex, gnomeSkin, playerIcon.sprite);
            isReady = true;
        }

        private void SetPlayerIndex(int index)
        {
            playerIndex = index;
            gnomeSkin = gnomeSelector.GetNextGnome();

            gnomeImage.sprite = gnomeSkin.GnomeSprite;
            gnomeImage.enabled = true;
            gnomeImage.SetNativeSize();

            GetComponent<InputSystemUIInputModule>().enabled = true;
        }
    }
}
