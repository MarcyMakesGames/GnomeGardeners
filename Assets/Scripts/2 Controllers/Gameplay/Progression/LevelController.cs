using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using System;

namespace GnomeGardeners
{
    public class LevelController : MonoBehaviour
    {
        public float availableTime = 120f;
        public int requiredScore = 1000;

        private int currentScore;
        private float restTime;
        private float timeAtStart;
        private TimerUI timerUI;
        private Scoreboard scoreboardUI;
        private bool isActive;

        private VoidEventChannelSO OnLevelStartEvent;
        private VoidEventChannelSO OnLevelLoseEvent;
        private VoidEventChannelSO OnLevelWinEvent;
        private IntEventChannelSO OnScoreAddEvent;
        private FloatEventChannelSO OnCurrentLevelTimeEvent;
        private IntEventChannelSO OnCurrentLevelCurrentScore;
        private IntEventChannelSO OnCurrentLevelRequiredScore;


        public float RestTime { get => restTime; }
        public int CurrentScore { get => currentScore; set => currentScore = value; }

        #region Unity Methods

        private void Awake()
        {
            OnLevelStartEvent = Resources.Load<VoidEventChannelSO>("Channels/LevelStartEC");
            OnLevelLoseEvent = Resources.Load<VoidEventChannelSO>("Channels/LevelLoseEC");
            OnLevelWinEvent = Resources.Load<VoidEventChannelSO>("Channels/LevelWinEC");
            OnScoreAddEvent = Resources.Load<IntEventChannelSO>("Channels/ScoreAddEC");
            OnCurrentLevelTimeEvent = Resources.Load<FloatEventChannelSO>("Channels/CurrentLevelTimeEC");
            OnCurrentLevelCurrentScore = Resources.Load<IntEventChannelSO>("Channels/CurrentLevelCurrentScoreEC");
            OnCurrentLevelRequiredScore = Resources.Load<IntEventChannelSO>("Channels/CurrentLevelRequiredScoreEC");
            OnScoreAddEvent.OnEventRaised += AddToScore;
        }

        private void Start()
        {
            timeAtStart = GameManager.Instance.Time.ElapsedTime;
            restTime = availableTime;
            timerUI = FindObjectOfType<TimerUI>();
            scoreboardUI = FindObjectOfType<Scoreboard>();
            currentScore = 0;

            OnLevelStartEvent.RaiseEvent();

            isActive = true;

            DebugLogger.Log(this, "Level Start.");
        }

        private void Update()
        {
            CalculateTime();

            CheckLoseCondition();

            CheckWinCondition();

            OnCurrentLevelCurrentScore.RaiseEvent(currentScore);
            OnCurrentLevelRequiredScore.RaiseEvent(requiredScore);
        }

        private void OnDestroy()
        {
            OnScoreAddEvent.OnEventRaised -= AddToScore;
        }

        #endregion

        #region Private Methods

        private void CalculateTime()
        {
            restTime = availableTime - GameManager.Instance.Time.GetTimeSince(timeAtStart);
            OnCurrentLevelTimeEvent.RaiseEvent(restTime);
        }

        private void AddToScore(int value)
        {
            currentScore += value;
            scoreboardUI.UpdateUI(currentScore);
        }

        private void CheckWinCondition()
        {
            if (currentScore >= requiredScore && isActive)
            {
                GameManager.Instance.LevelManager.lastTotalScore = currentScore;
                GameManager.Instance.LevelManager.lastRequiredScore = requiredScore;
                OnLevelWinEvent.RaiseEvent();
                DebugLogger.Log(this, "Level won!");
                isActive = false;
            }
        }

        private void CheckLoseCondition()
        {
            if (restTime <= 0f && isActive)
            {
                GameManager.Instance.LevelManager.lastTotalScore = currentScore;
                GameManager.Instance.LevelManager.lastRequiredScore = requiredScore;
                OnLevelLoseEvent.RaiseEvent();
                DebugLogger.Log(this, "Level lost!");
                isActive = false;
            }
        }

        #endregion

    }
}
