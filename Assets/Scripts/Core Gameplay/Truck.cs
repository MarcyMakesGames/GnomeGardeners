using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Truck : MonoBehaviour , ITruck
{
    public int TotalScore { get ; set; }

    #region Unity Methods

    void Start()
    {
        TotalScore = 0;
    }

    #endregion

    #region Public Methods

    public void AddScore(int score)
    {
        TotalScore += score;

        FindObjectOfType<Scoreboard>().UpdateUI(TotalScore);
    }

    #endregion
}
