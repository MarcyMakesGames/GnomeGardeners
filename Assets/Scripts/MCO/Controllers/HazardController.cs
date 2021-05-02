using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardController : MonoBehaviour
{
    [SerializeField]
    private List<Hazard> hazards;
    [SerializeField]
    private bool randomizeHazards;
    [SerializeField]
    private float hazardDelayTime;
    [SerializeField]
    private List<Transform> spawnLocations;
    [SerializeField]
    private List<Transform> despawnLocations;

    private int spawnDespawnIndex;

    private float timeBetweenHazards;

    private int currentHazardIndex = 0;

    private float currentHazardTimer = 0f;

    public ScriptableObject CurrentHazard { get => hazards[currentHazardIndex]; }

    public delegate void OnHazardChange();
    public event OnHazardChange HazardChanged;

    #region Unity Methods
    private void Awake()
    {
        GameManager.Instance.HazardController = this;
    }

    private void Start()
    {
        currentHazardTimer = GameManager.Instance.Time.ElapsedTime;
    }

    private void Update()
    {
        HazardCountdown();
    }
    #endregion

    #region Private Methods
    private void HazardCountdown()
    {
        if(hazardDelayTime > 0f)
        {
            hazardDelayTime -= Time.deltaTime;
            return;
        }

        if (!randomizeHazards && currentHazardIndex >= hazards.Count)
            return; 

        if (GameManager.Instance.Time.GetTimeSince(currentHazardTimer) >= timeBetweenHazards)
        {
            if(randomizeHazards)
            {
                GetRandomHazard().SpawnHazard(GetRandomSpawn(), GetRandomDespawn());
                timeBetweenHazards = hazards[currentHazardIndex].HazardDuration;
                currentHazardIndex++;
            }
            else
            {
                GetNextHazard().SpawnHazard(GetRandomSpawn(), GetRandomDespawn());
                timeBetweenHazards = hazards[currentHazardIndex].HazardDuration;
                currentHazardIndex++;
            }
            //HazardChanged();
            currentHazardTimer = GameManager.Instance.Time.ElapsedTime;
        }
    }

    private Hazard GetRandomHazard()
    {
        currentHazardIndex = Random.Range(0, hazards.Count);
        return hazards[currentHazardIndex];
    }

    private Hazard GetNextHazard()
    {
        return hazards[currentHazardIndex];
    }
    #endregion

    private Vector3 GetRandomSpawn()
    {
        spawnDespawnIndex = Random.Range(0, spawnLocations.Count);
        return spawnLocations[spawnDespawnIndex].position;
    }

    private Vector3 GetRandomDespawn()
    {
        return despawnLocations[spawnDespawnIndex].position;
    }
}
