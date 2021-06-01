using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GnomeGardeners
{
    public class ScoreSliderUI : CoreUIElement<int>
    {
        [Header("Designers")]
        [SerializeField] private float animationTime = 2f;
        [Header("Programmers")]
        [SerializeField] private Slider scoreSlider;

        private IntEventChannelSO OnCurrentLevelCurrentScore;
        private IntEventChannelSO OnCurrentLevelRequiredScore;
        private VoidEventChannelSO OnLevelEnd;

        private int totalScore;
        private int animationValue;

        #region Unity Methods
        
        private void Awake()
        {
            OnCurrentLevelCurrentScore = Resources.Load<IntEventChannelSO>("Channels/CurrentLevelCurrentScoreEC");
            OnCurrentLevelRequiredScore = Resources.Load<IntEventChannelSO>("Channels/CurrentLevelRequiredScoreEC");
            OnLevelEnd = Resources.Load<VoidEventChannelSO>("Channels/LevelEndEC");
            OnCurrentLevelCurrentScore.OnEventRaised += SetTotalScore;
            OnCurrentLevelRequiredScore.OnEventRaised += UpdateSliderMinMax;
            OnLevelEnd.OnEventRaised += PlayAnimation;

            totalScore = 0;
            animationValue = 0;
        }
        
        private void OnDestroy()
        {
            OnCurrentLevelCurrentScore.OnEventRaised -= SetTotalScore;
            OnCurrentLevelRequiredScore.OnEventRaised -= UpdateSliderMinMax;
            OnLevelEnd.OnEventRaised -= PlayAnimation;
        }

        #endregion

        public override void UpdateUI(int primaryData)
        {
            UpdateSliderValue(scoreSlider, primaryData);
        }

        #region Private Methods

        private void PlayAnimation()
        {
            StartCoroutine(FillUp());
        }

        private IEnumerator FillUp()
        {
            int animationFrameCount = Mathf.FloorToInt(animationTime * 30);
            for (int i = 0; i < animationFrameCount; i++)
            {
                var value = totalScore / animationFrameCount;
                animationValue += value;
                UpdateSliderValue(scoreSlider, animationValue);
                yield return new WaitForFixedUpdate();
            }
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
            return true;
        }

        private void SetTotalScore(int value)
        {
            totalScore = value;
        }

        [ContextMenu("Test Animation")]
        private void TestAnimation()
        {
            totalScore = 100;
            PlayAnimation();
        }
        
        #endregion
    }
}
