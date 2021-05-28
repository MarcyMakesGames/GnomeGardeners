using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
    public class HazardManager : MonoBehaviour
    {
        [SerializeField] private List<HazardSO> hazards;
        [SerializeField] private float timeToFirstHazard;
        [SerializeField] private List<Transform> spawnLocations;
        [SerializeField] private List<Transform> despawnLocations;

        private int spawnDespawnIndex;
        private float timeBetweenHazards;
        private int nextHazardIndex;
        private float currentHazardTimer = 0f;
        private bool isSpawningHazards = true;
        private Vector3 movementModifier;
        private HazardSO nextHazard;
        private HazardSO currentHazard;

        public ScriptableObject CurrentHazard { get => hazards[nextHazardIndex]; }
        public Vector3 MovementModifier { get => movementModifier; set => movementModifier = value; }
        
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
            if (GameManager.Instance.HazardManager == null)
                GameManager.Instance.HazardManager = this;
            
            OnLevelStart.OnEventRaised += StartSpawningHazards;
            OnLevelLose.OnEventRaised += StopSpawningHazards;
            OnLevelWin.OnEventRaised += StopSpawningHazards;
        }

        private void Start()
        {
            nextHazardIndex = 0;
            if (isSpawningHazards)
            {
                currentHazardTimer = GameManager.Instance.Time.ElapsedTime;
                nextHazard = hazards[nextHazardIndex];
            }

            OnLevelStart.OnEventRaised += RaiseFirstHazardEvent;
        }

        private void Update()
        {
            if (isSpawningHazards)
                HazardCountdown();
        }

        private void OnDisable()
        {
            OnLevelStart.OnEventRaised -= StartSpawningHazards;
            OnLevelLose.OnEventRaised -= StopSpawningHazards;
            OnLevelWin.OnEventRaised -= StopSpawningHazards;
            OnLevelStart.OnEventRaised -= RaiseFirstHazardEvent;
            GameManager.Instance.HazardManager = null;
        }

        #endregion

        #region Private Methods
        private void HazardCountdown()
        {
            if(timeToFirstHazard > 0f || hazards.Count == 0)
            {
                timeToFirstHazard -= GameManager.Instance.Time.DeltaTime;
                return;
            }

            if (GameManager.Instance.Time.GetTimeSince(currentHazardTimer) >= timeBetweenHazards)
            {
                currentHazard = nextHazard;
                currentHazard.SpawnHazard(GetRandomSpawn(), GetRandomDespawn());
                if (nextHazardIndex >= hazards.Count - 1)
                    nextHazardIndex = 0;
                else
                    nextHazardIndex++;
                nextHazard = hazards[nextHazardIndex];
                timeBetweenHazards = nextHazard.HazardDuration;
                OnNextHazard.RaiseEvent(nextHazard.Icon, timeBetweenHazards, currentHazard.HazardDuration);
                currentHazardTimer = GameManager.Instance.Time.ElapsedTime;
            }
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

        private void RaiseFirstHazardEvent()
        {
            OnNextHazard.RaiseEvent(nextHazard.Icon, nextHazard.HazardDuration, timeToFirstHazard);
        }

        private void StartSpawningHazards() => isSpawningHazards = true;

        private void StopSpawningHazards() => isSpawningHazards = false;

        #endregion
    }
}
