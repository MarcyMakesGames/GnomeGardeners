using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Truck : MonoBehaviour , ITruck
{
    public int TotalScore { get ; set; }

    void Start()
    {
        TotalScore = 0;
    }

    public void AddScore(int score)
    {
        TotalScore += score;

        FindObjectOfType<Scoreboard>().UpdateUI(TotalScore);
    }
}
