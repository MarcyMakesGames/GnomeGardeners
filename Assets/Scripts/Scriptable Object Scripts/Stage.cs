using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stage", menuName = "Plants/Stage")]
public class Stage : ScriptableObject
{
    public string stageName;
    public PlantStage specifier;
    public int index;
    public Sprite sprite;
    public Need need;
    public float timeToNextStage;
    public bool isDeliverable;
    public int pointValue;
    public bool IsReady()
    {
        return need.IsFulfilled;
    }

    public void SatisfyNeed(NeedType type, float value)
    {
        if(need.type == type)
            need.Satisfy(value);
    }
}
