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
        private bool isActive;
        private bool hasStarted;

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
            timeAtStart = 0f;
            restTime = availableTime;
            currentScore = 0;
            isActive = false;
            hasStarted = false;
        }

        private void OnDestroy()
        {
            OnScoreAddEvent.OnEventRaised -= AddToScore;
        }

        #endregion
        
        #region Public Methods
        
        public void LevelStart()
        {
            OnLevelStartEvent.RaiseEvent();

            currentScore = 0;
            timeAtStart = GameManager.Instance.Time.ElapsedTime;
            restTime = availableTime;

            isActive = true;
            hasStarted = true;
        }
        
        public IEnumerator UpdateLevel()
        {
            while (isActive)
            {
                CalculateTime();

                CheckLoseCondition();

                CheckWinCondition();

                OnCurrentLevelCurrentScore.RaiseEvent(currentScore);
                OnCurrentLevelRequiredScore.RaiseEvent(requiredScore);
                yield return null;
            }
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
