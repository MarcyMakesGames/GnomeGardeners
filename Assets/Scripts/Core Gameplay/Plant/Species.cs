using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Species
{
    public string name;
    public List<Stage> stages;

    public Species(string name)
    {
        this.name = name;
    }

    public Stage NextStage(int current)
    {
        int next = current + 1;
        if (next < stages.Count)
            return stages[next];
        else
            return stages[stages.Count-1];
    }
}
