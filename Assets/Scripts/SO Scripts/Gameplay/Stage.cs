using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stage", menuName = "Plants/Stage")]
public class Stage : ScriptableObject
{
    public string stageName;
    public PlantStage specifier;
    public Sprite sprite;
    public Need need;
    public Vector3 popUpPositionOffset;
    public float timeToFulfillNeed;
    public float timeToGrow;
    public bool isHarvestable;
    public bool isPlantable;
    public int pointValue;
}
