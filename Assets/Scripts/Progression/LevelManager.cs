using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Level activeLevel;
    private List<Level> levels;

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
    }

    void Update()
    {
        activeLevel.Update();
    }

    public void SetLevelActive(int index)
    {
        if(index >= levels.Count) { return; }
        activeLevel = levels[index];
        activeLevel.isCurrent = true;
    }
}
