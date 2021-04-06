using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Truck : MonoBehaviour , ITruck
{
    public int TotalScore { get ; set; }

    // Start is called before the first frame update
    void Start()
    {
        TotalScore = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float time = GameManager.Instance.Time.ElapsedTime;
        if ( time % 5f == 0f)
        {
            Debug.Log(TotalScore.ToString());
        }
    }

    public void AddScore(int score)
    {
        TotalScore += score;
    }
}
