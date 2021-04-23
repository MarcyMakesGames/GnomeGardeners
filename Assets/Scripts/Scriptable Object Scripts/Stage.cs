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
    public List<Need> needs;
    public float timeToNextStage;
    public bool isDeliverable;
    public int pointValue;
    public bool IsReady()
    {
        foreach (Need need in needs)
        {
            if (!need.IsFulfilled)
            {
                return false;
            }
        }
        return true;
    }

    public void SatisfyNeed(int index, float value)
    {
        needs[index].Satisfy(value);
    }

    public void SatisfyNeeds(float value)
    {
        foreach (Need need in needs)
        {
            need.Satisfy(value);
        }
    }
}
