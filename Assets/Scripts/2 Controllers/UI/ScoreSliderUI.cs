using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GnomeGardeners
{
    public class ScoreSliderUI : CoreUIElement<int>
    {
        [SerializeField] private Slider scoreSlider;

        private IntEventChannelSO OnCurrentLevelCurrentScore;
        private IntEventChannelSO OnCurrentLevelRequiredScore;

        private void Awake()
        {
            OnCurrentLevelCurrentScore = Resources.Load<IntEventChannelSO>("Channels/CurrentLevelCurrentScoreEC");
            OnCurrentLevelRequiredScore = Resources.Load<IntEventChannelSO>("Channels/CurrentLevelRequiredScoreEC");
            OnCurrentLevelCurrentScore.OnEventRaised += UpdateUI;
            OnCurrentLevelRequiredScore.OnEventRaised += UpdateSliderMinMax;
        }

        private void OnDestroy()
        {
            OnCurrentLevelCurrentScore.OnEventRaised -= UpdateUI;
            OnCurrentLevelRequiredScore.OnEventRaised -= UpdateSliderMinMax;
        }

        public override void UpdateUI(int primaryData)
        {
            UpdateSliderValue(scoreSlider, primaryData);
        }

        private void UpdateSliderMinMax(int max)
        {
            UpdateSliderMinMax(scoreSlider, 0, max);
        }

        protected override bool ClearedIfEmpty(int newData)
        {
            if (newData != 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
