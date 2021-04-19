using UnityEngine;
using TMPro;

public class TimerUI : CoreUIElement<float>
{
    [SerializeField] private TMP_Text timerText;

    public override void UpdateUI(float primaryData)
    {
        UpdateTimeAsString(timerText, primaryData);
    }

    protected override bool ClearedIfEmpty(float newData)
    {
        if (newData != null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
