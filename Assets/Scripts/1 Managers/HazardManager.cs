using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
    public class HazardManager : MonoBehaviour
    {
        [SerializeField]
        private List<HazardSO> hazards;
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
        private bool isSpawningHazards = false;

        public ScriptableObject CurrentHazard { get => hazards[currentHazardIndex]; }

        public delegate void OnHazardChange();
        public event OnHazardChange HazardChanged;

        public VoidEventChannelSO OnLevelStart;
        public VoidEventChannelSO OnLevelLose;
        public VoidEventChannelSO OnLevelWin;

        #region Unity Methods
        private void Awake()
        {
            Configure();
        }

        private void Start()
        {
            currentHazardTimer = GameManager.Instance.Time.ElapsedTime;
        }

        private void Update()
        {
            if (isSpawningHazards)
            {
                HazardCountdown();
            }
        }

        private void OnDisable()
        {
            Dispose();
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

        private void Configure()
        {
            if (GameManager.Instance.HazardManager == null)
            {
                GameManager.Instance.HazardManager = this;
            }
            OnLevelStart.OnEventRaised += StartSpawningHazards;
            OnLevelLose.OnEventRaised += StopSpawningHazards;
            OnLevelWin.OnEventRaised += StopSpawningHazards;
        }
        private void Dispose()
        {
            OnLevelStart.OnEventRaised -= StartSpawningHazards;
            OnLevelLose.OnEventRaised -= StopSpawningHazards;
            OnLevelWin.OnEventRaised -= StopSpawningHazards;
        }

        private HazardSO GetRandomHazard()
        {
            currentHazardIndex = Random.Range(0, hazards.Count);
            return hazards[currentHazardIndex];
        }

        private HazardSO GetNextHazard()
        {
            return hazards[currentHazardIndex];
        }

        private Vector3 GetRandomSpawn()
        {
            spawnDespawnIndex = Random.Range(0, spawnLocations.Count);
            return spawnLocations[spawnDespawnIndex].position;
        }

        private Vector3 GetRandomDespawn()
        {
            return despawnLocations[spawnDespawnIndex].position;
        }

        private void StartSpawningHazards()
        {
            isSpawningHazards = true;
        }

        private void StopSpawningHazards()
        {
            isSpawningHazards = false;
        }

        #endregion
    }
}
