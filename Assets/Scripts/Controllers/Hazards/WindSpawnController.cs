using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSpawnController : HazardSpawnController
{
    [SerializeField]
    private GameObject windPrefab;
    [SerializeField]
    private List<Transform> spawnAreas;

    private float startTime;
    private float spawnTime;

    private void Awake()
    {
        InitWindSpawner();
    }

    private void Update()
    {
        CountdownTimer();
    }

    private void InitWindSpawner()
    {
        startTime = GameManager.Instance.Time.ElapsedTime;
        spawnTime = GameManager.Instance.Time.ElapsedTime;

        transform.position = spawnLocation;
        transform.position = Vector3.RotateTowards(transform.position, despawnLocation, 10000f * Time.deltaTime, 1000f);
    }

    private void CountdownTimer()
    {
        if (GameManager.Instance.Time.GetTimeSince(startTime) >= hazardDuration)
            Destroy(gameObject);

        if (GameManager.Instance.Time.GetTimeSince(spawnTime) >= timeBetweenSpawns)
        {
            SpawnWindObject();
            Debug.Log("Spawning an object");
            spawnTime = GameManager.Instance.Time.ElapsedTime;
        }
    }

    private void SpawnWindObject()
    {
        var randomSpawnPos = spawnAreas[Random.Range(0, spawnAreas.Count)];
        var positionDifferential = randomSpawnPos.position - transform.position;
        var targetDespawn = despawnLocation + positionDifferential;

        GameObject windGust = Instantiate(windPrefab, randomSpawnPos.position, transform.rotation);
        WindObjectController windObjController = windGust.GetComponent<WindObjectController>();
        windObjController.despawnLocation = targetDespawn;
        windObjController.moveSpeed = spawnObjMoveSpeed;
    }
}
