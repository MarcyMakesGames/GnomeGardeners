using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HazardElementName", menuName = "Hazard Element")]
public class HazardElementSO : ScriptableObject
{
    [SerializeField]
    private int uniqueID;
    [SerializeField]
    private Direction direction;
    [SerializeField]
    private GameObject hazardSpawner;
    [SerializeField]
    private int hazardSpawnerAmount;
    [SerializeField]
    private float hazardDuration;
    [SerializeField] 
    private float timeBetweenSpawns;
    [SerializeField]
    private float spawnedObjMoveSpeed;

    public int UniqueID { get => uniqueID; }
    public Direction Direction { get => direction; }
    public float Duration { get => hazardDuration;  }

    public void SpawnElement(Vector3 spawnLocation, Vector3 despawnLocation) 
    {
        for(int i = 0; i < hazardSpawnerAmount; ++i)
        {
            //THIS NEEDS TO BE GOTTEN FROM THE OBJECT POOL IN THE FUTURE
            var hazardSpawnerInstance = Instantiate(this.hazardSpawner, spawnLocation, Quaternion.identity, GameManager.Instance.HazardManager.transform);
            hazardSpawnerInstance.transform.position = spawnLocation;

            var hazardSpawner = hazardSpawnerInstance.GetComponent<HazardSpawner>();
            hazardSpawner.InitSpawner(spawnLocation, despawnLocation, hazardDuration, timeBetweenSpawns, spawnedObjMoveSpeed);
        }
    }
}