using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Stage
{
    public Sprite sprite;
    public PlantStage specifier;
    [SerializeField] private int index;
    [SerializeField] private float timeToNextStage = 5f;
    [SerializeField] private List<Need> needs;
    [SerializeField] private int score;
    [HideInInspector] public string name;


    public float TimeToNextStage { get => timeToNextStage; }
    public int Index { get => index; }
    public int Score { get => score;}

    public Stage(PlantStage specifier, int index)
    {
        name = specifier.ToString();
        this.specifier = specifier;
    }

    public bool IsReady() 
    {
        foreach(Need need in needs)
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
        foreach(Need need in needs)
        {
            need.Satisfy(value);
        }
    }
}
