using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Plant Event", menuName = "Events/Plant Event Channel")]
public class PlantEventChannelSO : ScriptableObject
{
    public delegate void PlantAction(Plant plant);
    public PlantAction OnEventRaised;

    public void RaiseEvent(Plant plant)
    {
        OnEventRaised?.Invoke(plant);
    }
}
