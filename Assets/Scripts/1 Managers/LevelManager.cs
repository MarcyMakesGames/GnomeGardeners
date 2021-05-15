using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    /* 
     * The level manager handles scores and other level specific data across scenes.
     */

    public int lastTotalScore;
    public int lastRequiredScore;



    #region Unity Methods
    private void Awake()
    {
        if (GameManager.Instance.LevelManager == null)
        {
            Configure();
        }

    }

    #endregion

    #region Private Methods
    private void Configure()
    {
        GameManager.Instance.LevelManager = this;
    }

    #endregion
}
