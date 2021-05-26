using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GnomeGardeners
{
    public class GnomeMenuController : MonoBehaviour
    {
        [SerializeField] private Button readyButton;
        [SerializeField] private Image gnomeImage;
        private GnomeSelectorController gnomeSelector;
        private GnomeSkinObject gnomeSkin;
        private int PlayerIndex;


        private float inputDelayTime = 0.15f;
        private bool inputEnabled;

        private void Start()
        {
            gnomeSelector = FindObjectOfType<GnomeSelectorController>();
            gnomeSkin = gnomeSelector.GetNextGnome();
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

            readyButton.GetComponent<TMP_Text>().color = Color.red;
            GameManager.Instance.PlayerConfigManager.ReadyPlayer(PlayerIndex, gnomeSkin);
        }
    }
}
