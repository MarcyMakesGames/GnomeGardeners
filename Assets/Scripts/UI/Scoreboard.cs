using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Scoreboard : CoreUIElement<int>
{
    [SerializeField] protected TMP_Text scoreText;

    protected int currentScore;

    public override void UpdateUI(int primaryData)
    {
        if (ClearedIfEmpty(primaryData))
            return;

        currentScore += primaryData;

        UpdateNumericText(scoreText, "{0}", primaryData);
    }

    protected override bool ClearedIfEmpty(int newData)
    {
        if (newData != 0)
            return false;
        return true;
    }
}
