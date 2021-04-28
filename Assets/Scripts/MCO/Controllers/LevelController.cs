using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;

public class LevelController : MonoBehaviour
{
    private bool debug = true;

    public float availableTime;
    private float restTime;
    private float timeAtStart;
    private TimerUI timerUI;

    public VoidEventChannelSO OnLevelStartEvent;
    public VoidEventChannelSO OnLevelEndEvent;

    public float RestTime { get => restTime; }

    #region Unity Methods

    private void Start()
    {
        timeAtStart = GameManager.Instance.Time.ElapsedTime;
        restTime = availableTime;
        timerUI = FindObjectOfType<TimerUI>();
        OnLevelStartEvent.RaiseEvent();

        Log("Level Start.");
    }

    private void Update()
    {
        CalculateTime();

        if (restTime <= 0f)
        {
            // on level end event
            OnLevelEndEvent.RaiseEvent();
            Log("Level End.");
        }
    }

    #endregion

    #region Private Methods

    private void CalculateTime()
    {
        restTime = availableTime - GameManager.Instance.Time.GetTimeSince(timeAtStart);

        if (timerUI != null)
            timerUI.UpdateUI(RestTime);
    }

    private void Log(string msg)
    {
        if (!debug) { return; }
        Debug.Log("[LevelController]: " + msg);
    }

    #endregion

}
