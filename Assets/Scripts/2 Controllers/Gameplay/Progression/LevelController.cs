using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using System;

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

    public VoidEventChannelSO OnLevelStartEvent;
    public VoidEventChannelSO OnLevelLoseEvent;
    public VoidEventChannelSO OnLevelWinEvent;
    public IntEventChannelSO OnScoreAddEvent;

    public float RestTime { get => restTime; }
    public int TotalScore { get => totalScore; set => totalScore = value; }

    #region Unity Methods

    private void Awake()
    {
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

        Log("Level Start.");
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

        if (timerUI != null)
            timerUI.UpdateUI(RestTime);
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
            Log("Level won!");
        }
    }

    private void CheckLoseCondition()
    {
        if (restTime <= 0f)
        {
            GameManager.Instance.LevelManager.lastTotalScore = totalScore;
            GameManager.Instance.LevelManager.lastRequiredScore = requiredScore;
            OnLevelLoseEvent.RaiseEvent();
            Log("Level lost!");
        }
    }

    private void Log(string msg)
    {
        if (!debug) { return; }
        Debug.Log("[LevelController]: " + msg);
    }

    #endregion

}
