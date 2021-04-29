using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTime : MonoBehaviour
{
    private float currentTimer = 0f;
    private bool pauseTimer = false;
    public float ElapsedTime { get => currentTimer; }
    public bool PauseTimer { get => pauseTimer; set => pauseTimer = value; }

    #region Unity Methods

    private void Awake()
    {
        if(GameManager.Instance.Time == null)
        {
            GameManager.Instance.Time = this;
        }
    }

    private void Update()
    {
        CountUp();
    }

    #endregion

    #region Public Methods

    public float GetTimeSince(float time)
    {
        return ElapsedTime - time;
    }

    #endregion

    #region Private Methods

    private void CountUp()
    {
        if (!pauseTimer)
            currentTimer += Time.deltaTime;
    }

    #endregion
}
