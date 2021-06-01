using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

namespace GnomeGardeners
{
    public class LevelController : MonoBehaviour
    {
        public float availableTime = 120f;
        public int requiredScore = 1000;
        public GameObject tutorialMenu;

        private int currentScore;
        private float restTime;
        private float timeAtStart;
        private bool isActive;
        private bool hasStarted;
        private bool hasBeenCompleted;

        private VoidEventChannelSO OnLevelStart;
        private VoidEventChannelSO OnLevelLose;
        private VoidEventChannelSO OnLevelWin;
        private VoidEventChannelSO OnLevelEnd;
        private IntEventChannelSO OnScoreAdd;
        private FloatEventChannelSO OnCurrentLevelTime;
        private IntEventChannelSO OnCurrentLevelCurrentScore;
        private IntEventChannelSO OnCurrentLevelRequiredScore;
        
        public float RestTime { get => restTime; }
        public int CurrentScore { get => currentScore; set => currentScore = value; }

        public bool HasBeenCompleted => hasBeenCompleted;

        #region Unity Methods

        private void Awake()
        {
            OnLevelStart = Resources.Load<VoidEventChannelSO>("Channels/LevelStartEC");
            OnLevelLose = Resources.Load<VoidEventChannelSO>("Channels/LevelLoseEC");
            OnLevelWin = Resources.Load<VoidEventChannelSO>("Channels/LevelWinEC");
            OnLevelEnd = Resources.Load<VoidEventChannelSO>("Channels/LevelEndEC");
            OnScoreAdd = Resources.Load<IntEventChannelSO>("Channels/ScoreAddEC");
            OnCurrentLevelTime = Resources.Load<FloatEventChannelSO>("Channels/CurrentLevelTimeEC");
            OnCurrentLevelCurrentScore = Resources.Load<IntEventChannelSO>("Channels/CurrentLevelCurrentScoreEC");
            OnCurrentLevelRequiredScore = Resources.Load<IntEventChannelSO>("Channels/CurrentLevelRequiredScoreEC");
            OnScoreAdd.OnEventRaised += AddToScore;
        }

        private void Start()
        {
            timeAtStart = 0f;
            restTime = availableTime;
            currentScore = 0;
            isActive = false;
            hasStarted = false;
            hasBeenCompleted = false;
        }

        private void OnDestroy()
        {
            GameManager.Instance.PoolController.SetPoolObjectsInactive();
            OnScoreAdd.OnEventRaised -= AddToScore;
        }

        #endregion
        
        #region Public Methods
        
        public void LevelStart()
        {
            OnLevelStart.RaiseEvent();

            currentScore = 0;
            GameManager.Instance.Time.ResetTimer();
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

                yield return null;
            }
        }
        
        #endregion

        #region Private Methods
        
        private void CalculateTime()
        {
            restTime = availableTime - GameManager.Instance.Time.GetTimeSince(timeAtStart);
            OnCurrentLevelTime.RaiseEvent(restTime);
        }

        private void AddToScore(int value)
        {
            currentScore += value;
            OnCurrentLevelCurrentScore.RaiseEvent(currentScore);
        }

        private void CheckWinCondition()
        {
            if (currentScore >= requiredScore && isActive)
            {
                OnLevelWin.RaiseEvent();
                OnLevelEnd.RaiseEvent();
                OnCurrentLevelCurrentScore.RaiseEvent(currentScore);
                OnCurrentLevelRequiredScore.RaiseEvent(requiredScore);
                isActive = false;
                hasBeenCompleted = true;
            }
        }

        private void CheckLoseCondition()
        {
            if (restTime <= 0f && isActive)
            {
                OnLevelLose.RaiseEvent();
                OnLevelEnd.RaiseEvent();
                OnCurrentLevelCurrentScore.RaiseEvent(currentScore);
                OnCurrentLevelRequiredScore.RaiseEvent(requiredScore);
                isActive = false;
            }
        }

        #endregion

    }
}
