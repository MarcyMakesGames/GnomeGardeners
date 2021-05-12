using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsectSpawner : HazardSpawner
{
    [SerializeField]
    private GameObject insectPrefab;
    [SerializeField]
    private int insectAmount = 1;

    private float startTime;
    private Insect[] insects;

    private void Awake()
    {
        startTime = GameManager.Instance.Time.ElapsedTime;
        transform.position = spawnPosition;
        insects = new Insect[insectAmount];
    }

    private void Start()
    {
        SpawnInsect();
    }

    private void SpawnInsect()
    {
        for(int i = 0; i < insectAmount; ++i)
        {
            var insectObject = Instantiate(insectPrefab, transform.position, transform.rotation);
            var insect = insectObject.GetComponent<Insect>();
            insect.despawnLocation = despawnPosition;
            insects[i] = insect;
        }
    }

    private void Update()
    {
        if (GameManager.Instance.Time.GetTimeSince(startTime) >= hazardDuration)
        {
            Despawn();
        }
    }

    private void Despawn()
    {
        foreach(Insect insect in insects)
        {
            insect.SetFleeing();
        }
        Destroy(gameObject);
    }
}
