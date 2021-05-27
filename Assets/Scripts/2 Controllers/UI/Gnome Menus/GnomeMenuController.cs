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
        private GnomeSelectorController gnomeSelector;
        private GnomeSkinObject gnomeSkin;
        private int playerIndex;
        public int PlayerIndex { get => playerIndex; set => SetPlayerIndex(value); }


        private float inputDelayTime = 0.15f;
        private bool inputEnabled;

        private void OnEnable()
        {
            gnomeSelector = FindObjectOfType<GnomeSelectorController>();
            gnomeImage.sprite = null;
        }

        private void Update()
        {
            if (Time.time > inputDelayTime)
                inputEnabled = true;
        }

        public void CycleGnomeSkin(bool right) 
        {
            if (right)
            {
                gnomeSkin = gnomeSelector.GetNextGnome(gnomeSkin);
            }
            else
            {
                gnomeSkin = gnomeSelector.GetPreviousGnome(gnomeSkin);
            }

            gnomeImage.sprite = gnomeSkin.GnomeSprite;
        }

        public void SetPlayerReady()
        {
            if (!inputEnabled)
                return;

            readyButton.GetComponentInChildren<TMP_Text>().color = Color.red;
            GameManager.Instance.PlayerConfigManager.ReadyPlayer(PlayerIndex, gnomeSkin);
        }

        private void SetPlayerIndex(int index)
        {
            playerIndex = index;
            gnomeSkin = gnomeSelector.GetNextGnome();

            gnomeImage.sprite = gnomeSkin.GnomeSprite;
            gnomeImage.enabled = true;

            GetComponent<InputSystemUIInputModule>().enabled = true;
        }
    }
}
