using System;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField]
    private float availableTime = 300f;
    private float restTime;
    private float timeAtStart = 0f;
    private int levelIndex;
    private bool hasStarted = false;

    private bool isCurrent;
    public bool IsCurrent { get => isCurrent; set => isCurrent = value; }

    public string GetTimeAsString()
    {
        int minutes = (int) Mathf.Floor(restTime / 60f);
        int seconds = (int) Mathf.Floor(restTime % 60f);
        return minutes.ToString() + ":" + seconds.ToString();
    }

    protected void Awake()
    {
        GameManager.Instance.Level = this;
        restTime = availableTime;
    }

    private void OnLevelStart()
    {
            timeAtStart = GameManager.Instance.Time.ElapsedTime;
    }

    private void Update()
    {
        if (isCurrent)
        {
            if (!hasStarted)
            {
                OnLevelStart();
                hasStarted = true;
            }
            CalculateTime();

            if(restTime <= 0f)
            {
                OnLevelEnd();
            }
        }
    }

    private void OnLevelEnd()
    {
#if UNITY_STANDALONE
        Application.Quit();
#endif
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void CalculateTime()
    {
        restTime = availableTime - GameManager.Instance.Time.GetTimeSince(timeAtStart);
    }

}
