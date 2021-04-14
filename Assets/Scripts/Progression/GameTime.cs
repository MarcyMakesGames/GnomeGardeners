using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTime : MonoBehaviour
{
    protected float currentTimer = 0f;
    protected bool pauseTimer = false;
    public float ElapsedTime { get => currentTimer; }
    public bool PauseTimer { get => pauseTimer; set => pauseTimer = value; }

    public float GetTimeSince(float time)
    {
        return ElapsedTime - time;
    }

    protected void Awake()
    {
        GameManager.Instance.Time = this;
    }

    protected void Update()
    {
        CountUp();
    }

    protected void CountUp()
    {
        if (!pauseTimer)
            currentTimer += Time.deltaTime;
    }
}
