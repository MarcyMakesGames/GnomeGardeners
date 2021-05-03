using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HazardSpawnController : MonoBehaviour
{
    protected Vector3 spawnLocation;
    protected Vector3 despawnLocation;
    protected float hazardDuration = 0f;
    protected float timeBetweenSpawns;
    protected float spawnObjMoveSpeed;
    
    public Vector3 SpawnLocation { set => spawnLocation = value; }
    public Vector3 DespawnLocation { set => despawnLocation = value; }
    public float HazardDuration { set => hazardDuration = value; }

    public void InitSpawner(Vector3 spawnLoc, Vector3 despawnLoc, float duration, float spawnDelay, float spawnMoveSpeed)
    {
        spawnLocation = spawnLoc;
        despawnLocation = despawnLoc;
        hazardDuration = duration;
        timeBetweenSpawns = spawnDelay;
        spawnObjMoveSpeed = spawnMoveSpeed;
    }
}
