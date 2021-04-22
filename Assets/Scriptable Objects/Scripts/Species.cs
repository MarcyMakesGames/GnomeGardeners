using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Species", menuName = "Plants/Species")]
public class Species : ScriptableObject
{
    public string speciesName;
    public List<Stage> stages;
    public Sprite decayedSprite;
    public bool isDamaging;
    public bool isSpawning;
    public bool isFragile;

    public Stage NextStage(int current)
    {
        int next = current + 1;
        if (next < stages.Count)
            return stages[next];
        else
            return stages[stages.Count - 1];
    }
}
