using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HazardName", menuName = "Hazard")]
public class Hazard : ScriptableObject
{
    [SerializeField]
    private List<HazardElement> hazardElements;
    private float hazardDuration = 0f;
    
    public float HazardDuration { get => hazardDuration; }

    public void SpawnHazard() 
    {
        foreach (HazardElement element in hazardElements)
        {
            element.SpawnElement();

            if(hazardDuration == 0f || element.Duration > hazardDuration)
                hazardDuration = element.Duration;
        }
    }
}
