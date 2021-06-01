using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GnomeGardeners
{
    public class Scoreboard : CoreUIElement<int>
    {
        [SerializeField] private TMP_Text scoreText;

        private IntEventChannelSO OnCurrentLevelCurrentScore;
        private VoidEventChannelSO OnLevelStart;

        private void Awake()
        {
            OnCurrentLevelCurrentScore = Resources.Load<IntEventChannelSO>("Channels/CurrentLevelCurrentScoreEC");
            OnLevelStart = Resources.Load<VoidEventChannelSO>("Channels/LevelStartEC");
            OnCurrentLevelCurrentScore.OnEventRaised += UpdateUI;
            OnLevelStart.OnEventRaised += ResetUI;
        }

        private void OnDestroy()
        {
            OnCurrentLevelCurrentScore.OnEventRaised -= UpdateUI;
            OnLevelStart.OnEventRaised -= ResetUI;
        }

        public override void UpdateUI(int primaryData)
        {
            if (ClearedIfEmpty(primaryData))
                return;

            UpdateNumericText(scoreText, "{0}", primaryData);
        }

        private void ResetUI()
        {
            UpdateNumericText(scoreText, "{0}", 0);
        }

        protected override bool ClearedIfEmpty(int newData)
        {
            if (newData != 0)
                return false;
            return true;
        }
    }
}
