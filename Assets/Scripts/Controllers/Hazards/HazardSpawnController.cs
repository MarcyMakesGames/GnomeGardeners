using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HazardSpawnController : MonoBehaviour
{
    protected Vector3 spawnLocation;
    protected Vector3 despawnLocation;
    protected float hazardDuration = 0f;
    
    public Vector3 SpawnLocation { set => spawnLocation = value; }
    public Vector3 DespawnLocation { set => despawnLocation = value; }
    public float HazardDuration { set => hazardDuration = value; }
}
