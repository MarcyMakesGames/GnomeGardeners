using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsectSpawnController : HazardSpawnController
{
    [SerializeField]
    private GameObject insectPrefab;

    private float startTime;

    private void Awake()
    {
        startTime = GameManager.Instance.Time.ElapsedTime;
        transform.position = spawnLocation;
    }

    private void Update()
    {
        if (GameManager.Instance.Time.GetTimeSince(startTime) >= hazardDuration)
            Destroy(gameObject);
    }
}
