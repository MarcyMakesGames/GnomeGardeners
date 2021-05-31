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
        private int spawnIterator;
        private float currentHazardTimer = 0f;
        private bool isSpawningHazards = true;
        private Vector3 movementModifier;
        private HazardSO currentHazard;

        private Queue<HazardSO> spawnQueue = new Queue<HazardSO>(2);
        private Stack<HazardSO> despawnStack = new Stack<HazardSO>();
        private HazardSO nextHazard;
        private HazardSO secondNextHazard;

        public ScriptableObject CurrentHazard { get => hazards[spawnIterator]; }
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
            spawnIterator = 0;
            if (isSpawningHazards)
            {
                currentHazardTimer = GameManager.Instance.Time.ElapsedTime;
                SetupQueue();
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
                currentHazard = spawnQueue.Dequeue();
                nextHazard = spawnQueue.Peek();
                secondNextHazard = EnqueueHazard();
                timeBetweenHazards = currentHazard.Duration;
                
                currentHazard.SpawnHazard(GetRandomSpawn(), GetRandomDespawn());               
                OnNextHazard.RaiseEvent(nextHazard.Icon, nextHazard.Duration, currentHazard.Duration, secondNextHazard.Duration);   

                
                despawnStack.Push(currentHazard);
                currentHazardTimer = GameManager.Instance.Time.ElapsedTime;
                EnqueueHazard();

            }
        }

        private void SetupQueue()
        {
            EnqueueHazard();
            nextHazard = spawnQueue.Peek();
            secondNextHazard = EnqueueHazard();
        }

        private HazardSO EnqueueHazard()
        {
            var enqueuedHazard = hazards[spawnIterator];
            spawnQueue.Enqueue(hazards[spawnIterator]);

            if (spawnIterator >= hazards.Count - 1)
                spawnIterator = 0;
            else
                spawnIterator++;

            return enqueuedHazard;
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
            OnNextHazard.RaiseEvent(nextHazard.Icon, nextHazard.Duration, timeToFirstHazard, secondNextHazard.Duration);
        }

        private void StartSpawningHazards() => isSpawningHazards = true;

        private void StopSpawningHazards() => isSpawningHazards = false;

        #endregion
    }
}
