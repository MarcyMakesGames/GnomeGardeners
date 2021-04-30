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
    private List<Transform> spawnLocations;
    [SerializeField]
    private List<Transform> despawnLocations;
    [SerializeField]
    private GameObject hazardElement;
    [SerializeField]
    private int hazardCount;
    [SerializeField]
    private float hazardDuration;

    public int UniqueID { get => uniqueID; }
    public Direction Direction { get => direction; }
    public float Duration { get => hazardDuration;  }

    public void SpawnElement() 
    {
        var spawnLoc = GetRandomSpawn();
        var despawnLoc = GetRandomDespawn();

        if(hazardElement.GetComponent<HazardObjectController>() == null)
        {
            Debug.Log("Hazard element " + hazardElement.name + " does not have a HazardObject script attached to it and did not spawn.");
            return;
        }
        
        for(int i = 0; i < hazardCount; i++)
        {
            //THIS NEEDS TO BE GOTTEN FROM THE OBJECT POOL IN THE FUTURE
            GameObject hazard = Instantiate(hazardElement, spawnLoc.position, Quaternion.identity, GameManager.Instance.HazardController.gameObject.transform);
            HazardObjectController hazardObjectController = hazard.GetComponent<HazardObjectController>();
            hazardObjectController.SpawnLocation = spawnLoc;
            hazardObjectController.DespawnLocation = despawnLoc;
            hazardObjectController.HazardDuration = hazardDuration;
        }
    }

    private Transform GetRandomSpawn()
    {
        return spawnLocations[Random.Range(0, spawnLocations.Count)];
    }

    private Transform GetRandomDespawn()
    {
        return despawnLocations[Random.Range(0, despawnLocations.Count)];
    }
}