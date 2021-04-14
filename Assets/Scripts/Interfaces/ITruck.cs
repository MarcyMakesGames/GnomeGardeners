using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITruck
{
    int TotalScore { get; set; }
    void AddScore(int score);
}
