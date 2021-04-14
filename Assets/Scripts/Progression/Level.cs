using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level
{
    public int index;
    public bool isCurrent;
    public float highscore;
    private float availableTime;
    private float restTime;
    private float timeAtStart;
    private bool hasStarted;
    private string sceneName;
    private string format;

    public string GetTimeAsString()
    {
        int minutes = (int)Mathf.Floor(restTime / 60f);
        int seconds = (int)Mathf.Floor(restTime % 60f);
        return minutes.ToString() + ":" + seconds.ToString(format);
    }
    public void Update()
    {
        if (isCurrent)
        {
            if (!hasStarted)
            {
                OnLevelStart();
                hasStarted = true;
            }
            CalculateTime();

            if (restTime <= 0f)
            {
                OnLevelEnd();
            }
        }
    }

    void OnLevelStart()
    {
        timeAtStart = GameManager.Instance.Time.ElapsedTime;
        restTime = availableTime;
        timeAtStart = 0f;
        hasStarted = false;
        format = "00";
        availableTime = 300f;
        restTime = 85f;
        Debug.Log(GetTimeAsString());
    }


    void OnLevelEnd()
    {
#if UNITY_STANDALONE
        Application.Quit();
#endif
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    void CalculateTime()
    {
        restTime = availableTime - GameManager.Instance.Time.GetTimeSince(timeAtStart);
    }
}
