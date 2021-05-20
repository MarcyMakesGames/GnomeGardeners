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
        private float timeToFirstHazard;
        [SerializeField]
        private List<Transform> spawnLocations;
        [SerializeField]
        private List<Transform> despawnLocations;

        private int spawnDespawnIndex;
        private float timeBetweenHazards;
        private int currentHazardIndex = 0;
        private float currentHazardTimer = 0f;
        private bool isSpawningHazards = true;
        private Vector3 movementModifier;
        private HazardSO nextHazard;
        private HazardSO currentHazard;

        public ScriptableObject CurrentHazard { get => hazards[currentHazardIndex]; }
        public Vector3 MovementModifier { get => movementModifier; set => movementModifier = value; }

        public delegate void OnHazardChange();
        public event OnHazardChange HazardChanged;

        private HazardEventChannelSO OnNextHazard;

        private VoidEventChannelSO OnLevelStart;
        private VoidEventChannelSO OnLevelLose;
        private VoidEventChannelSO OnLevelWin;

        #region Unity Methods
        private void Awake()
        {
            OnNextHazard = Resources.Load<HazardEventChannelSO>("Channels/NextHazardEC");
            OnLevelStart = Resources.Load<VoidEventChannelSO>("Channels/LevelStartEC");
            OnLevelLose = Resources.Load<VoidEventChannelSO>("Channels/LevelLoseEC");
            OnLevelWin = Resources.Load<VoidEventChannelSO>("Channels/LevelWinEC");
            Configure();
        }

        private void Start()
        {
            currentHazardTimer = GameManager.Instance.Time.ElapsedTime;
            if (randomizeHazards)
                nextHazard = GetRandomHazard();
            else
                nextHazard = GetNextHazard();

            OnNextHazard.RaiseEvent(nextHazard.Icon, nextHazard.HazardDuration, timeToFirstHazard);
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
            if(timeToFirstHazard > 0f || hazards.Count == 0)
            {
                timeToFirstHazard -= Time.deltaTime;
                return;
            }

            if (!randomizeHazards && currentHazardIndex >= hazards.Count)
                return; 

            if (GameManager.Instance.Time.GetTimeSince(currentHazardTimer) >= timeBetweenHazards)
            {
                currentHazard = nextHazard;
                if(randomizeHazards)
                {
                    nextHazard.SpawnHazard(GetRandomSpawn(), GetRandomDespawn());
                    timeBetweenHazards = hazards[currentHazardIndex].HazardDuration;
                    currentHazardIndex++;
                    nextHazard = GetRandomHazard();
                }
                else
                {
                    nextHazard.SpawnHazard(GetRandomSpawn(), GetRandomDespawn());
                    timeBetweenHazards = hazards[currentHazardIndex].HazardDuration;
                    currentHazardIndex++;
                    nextHazard = GetNextHazard();
                }
                OnNextHazard.RaiseEvent(nextHazard.Icon, nextHazard.HazardDuration, currentHazard.HazardDuration);
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

        private void StartSpawningHazards() => isSpawningHazards = true;

        private void StopSpawningHazards() => isSpawningHazards = false;

        #endregion
    }
}
