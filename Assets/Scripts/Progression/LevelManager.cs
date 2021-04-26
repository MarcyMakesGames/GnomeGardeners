using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Level activeLevel;
    private List<Level> levels;
    private TimerUI timerUI;

    #region Unity Methods

    void Awake()
    {
        GameManager.Instance.LevelManager = this;
    }

    void Start()
    {
        levels = new List<Level>
        {
            new Level()
        };
        activeLevel = levels[0];
        timerUI = FindObjectOfType<TimerUI>();
    }

    void Update()
    {
        activeLevel.Update();

        if(timerUI != null)
            timerUI.UpdateUI(activeLevel.RestTime);
    }

    #endregion

    #region Public Methods

    public void SetLevelActive(int index)
    {
        if(index >= levels.Count) { return; }
        activeLevel = levels[index];
        activeLevel.isCurrent = true;
    }

    #endregion
}
