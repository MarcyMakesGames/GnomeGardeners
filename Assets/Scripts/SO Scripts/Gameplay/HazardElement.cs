using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HazardElementName", menuName = "Hazard Element")]
public class HazardElement : ScriptableObject
{
    [SerializeField]
    private int uniqueID;
    [SerializeField]
    private Direction direction;
    [SerializeField]
    private GameObject hazardElement;
    [SerializeField]
    private int hazardCount;
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
        if(hazardElement.GetComponent<HazardSpawnController>() == null)
        {
            Debug.Log("Hazard element " + hazardElement.name + " does not have a HazardObject script attached to it and did not spawn.");
            return;
        }
        
        for(int i = 0; i < hazardCount; i++)
        {
            //THIS NEEDS TO BE GOTTEN FROM THE OBJECT POOL IN THE FUTURE
            GameObject hazard = Instantiate(hazardElement, spawnLocation, Quaternion.identity, GameManager.Instance.HazardController.gameObject.transform);
            hazard.transform.position = spawnLocation;
            HazardSpawnController hazardObjectController = hazard.GetComponent<HazardSpawnController>();
            hazardObjectController.InitSpawner(spawnLocation, despawnLocation, hazardDuration, timeBetweenSpawns, spawnedObjMoveSpeed);
        }
    }
}