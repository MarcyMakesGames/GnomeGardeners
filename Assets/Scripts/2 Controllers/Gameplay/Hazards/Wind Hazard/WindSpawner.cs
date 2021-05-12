using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSpawner : HazardSpawner
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

        transform.position = spawnPosition;
        transform.position = Vector3.RotateTowards(transform.position, despawnPosition, 10000f * Time.deltaTime, 1000f);
    }

    private void CountdownTimer()
    {
        if (GameManager.Instance.Time.GetTimeSince(startTime) >= hazardDuration)
            Destroy(gameObject);

        if (GameManager.Instance.Time.GetTimeSince(spawnTime) >= timeBetweenSpawns)
        {
            SpawnWindObject();
            spawnTime = GameManager.Instance.Time.ElapsedTime;
        }
    }

    private void SpawnWindObject()
    {
        var randomSpawnPos = spawnAreas[Random.Range(0, spawnAreas.Count)];
        var positionDifferential = randomSpawnPos.position - transform.position;
        var targetDespawn = despawnPosition + positionDifferential;

        var windObject = Instantiate(windPrefab, randomSpawnPos.position, transform.rotation);
        var wind = windObject.GetComponent<Wind>();
        wind.despawnLocation = targetDespawn;
        wind.moveSpeed = spawnObjMoveSpeed;
    }
}
