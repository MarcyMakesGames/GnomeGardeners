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

    public float RestTime { get { return restTime; } }

    #region Unity Methods

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

    #endregion

    #region Private Methods

    private void OnLevelStart()
    {
        timeAtStart = GameManager.Instance.Time.ElapsedTime;
        availableTime = 300f;
        restTime = availableTime;
        hasStarted = false;
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

#endregion
}
