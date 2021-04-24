using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Need", menuName = "Plants/Need")]
public class Need : ScriptableObject
{
    public NeedType type;
    public float threshold;
    public Sprite popUp;

    private float value;

    public bool IsFulfilled
    {
        get
        {
            if (value >= threshold)
                return true;
            else
                return false;
        }
    }

    public void Satisfy(float value)
    {
        this.value += value;
    }
}
