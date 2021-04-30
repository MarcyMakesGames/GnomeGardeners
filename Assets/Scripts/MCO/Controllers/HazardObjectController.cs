using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HazardObjectController : MonoBehaviour
{
    protected Transform spawnLocation = null;
    protected Transform despawnLocation = null;
    protected float hazardDuration = 0f;
    
    public Transform SpawnLocation { set => spawnLocation = value; }
    public Transform DespawnLocation { set => despawnLocation = value; }
    public float HazardDuration { set => hazardDuration = value; }
}
