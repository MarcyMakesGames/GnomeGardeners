using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using System;

namespace GnomeGardeners
{
    public class LevelController : MonoBehaviour
    {
        private bool debug = false;

        public float availableTime = 120f;
        public int requiredScore = 1000;

        private int totalScore;
        private float restTime;
        private float timeAtStart;
        private TimerUI timerUI;
        private Scoreboard scoreboardUI;

        private VoidEventChannelSO OnLevelStartEvent;
        private VoidEventChannelSO OnLevelLoseEvent;
        private VoidEventChannelSO OnLevelWinEvent;
        private IntEventChannelSO OnScoreAddEvent;
        private FloatEventChannelSO OnCurrentLevelTimeEvent;


        public float RestTime { get => restTime; }
        public int TotalScore { get => totalScore; set => totalScore = value; }

        #region Unity Methods

        private void Awake()
        {
            OnLevelStartEvent = Resources.Load<VoidEventChannelSO>("Channels/LevelStartEC");
            OnLevelLoseEvent = Resources.Load<VoidEventChannelSO>("Channels/LevelLoseEC");
            OnLevelWinEvent = Resources.Load<VoidEventChannelSO>("Channels/LevelWinEC");
            OnScoreAddEvent = Resources.Load<IntEventChannelSO>("Channels/ScoreAddEC");
            OnCurrentLevelTimeEvent = Resources.Load<FloatEventChannelSO>("Channels/CurrentLevelTimeEC");
            OnScoreAddEvent.OnEventRaised += AddToScore;
        }

        private void Start()
        {
            timeAtStart = GameManager.Instance.Time.ElapsedTime;
            restTime = availableTime;
            timerUI = FindObjectOfType<TimerUI>();
            scoreboardUI = FindObjectOfType<Scoreboard>();
            totalScore = 0;

            OnLevelStartEvent.RaiseEvent();

            DebugLogger.Log(this, "Level Start.");
        }

        private void Update()
        {
            CalculateTime();

            CheckLoseCondition();

            CheckWinCondition();
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
            totalScore += value;
            scoreboardUI.UpdateUI(totalScore);
        }

        private void CheckWinCondition()
        {
            if (totalScore >= requiredScore)
            {
                GameManager.Instance.LevelManager.lastTotalScore = totalScore;
                GameManager.Instance.LevelManager.lastRequiredScore = requiredScore;
                OnLevelWinEvent.RaiseEvent();
                DebugLogger.Log(this, "Level won!");
            }
        }

        private void CheckLoseCondition()
        {
            if (restTime <= 0f)
            {
                GameManager.Instance.LevelManager.lastTotalScore = totalScore;
                GameManager.Instance.LevelManager.lastRequiredScore = requiredScore;
                OnLevelLoseEvent.RaiseEvent();
                DebugLogger.Log(this, "Level lost!");
            }
        }

        #endregion

    }
}
