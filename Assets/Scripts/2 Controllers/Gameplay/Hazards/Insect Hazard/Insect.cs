using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
    public class Insect : Occupant
    {
        [HideInInspector] public Vector3 despawnLocation = new Vector3(0f, 0f, 0f);
        [HideInInspector] public List<Plant> excludedPlants;

        [Header("Designers")]
        [SerializeField] private float movementSpeed = 0.1f;
        [SerializeField] private float timeToEatPlant = 5f;
        [SerializeField] private int timesToResistShooing = 3;

        [Header("Programmers")]
        [SerializeField] private GameObject insectFront;
        [SerializeField] private GameObject insectBack;
        [SerializeField] private GameObject insectLeft;
        [SerializeField] private GameObject insectRight;

        private GridCell currentCell;
        private GridCell nextCell;
        private GridCell targetCell;
        private Plant targetPlant;
        private float timeAtReachedPlant;
        private bool isMoving;
        private bool isEating;
        private Direction direction;
        private int timesShooed;
        private GroundType currentGroundType;
        private AudioSource audioSource;

        private Animator animator;

        // State Machine (Behaviour)
        [HideInInspector]  public bool isSearchingPlant;
        [HideInInspector]  public bool isMovingToPlant;
        [HideInInspector]  public bool isFleeing;

        private int isIncomingHash;
        private int isEatingHash;
        private int isFleeingHash;
        private int hasTargetHash;

        // Event Channels
        public PlantEventChannelSO OnPlantTargeted;
        public PlantEventChannelSO OnPlantEaten;

        public GameObject AssociatedObject => gameObject;

        #region Unity Methods

        private void Awake()
        {
            OnPlantTargeted.OnEventRaised += ExcludePlant;
            OnPlantEaten.OnEventRaised += ForgetPlant;

            isIncomingHash = Animator.StringToHash("IsIncoming");
            isEatingHash = Animator.StringToHash("IsEating");
            isFleeingHash = Animator.StringToHash("IsFleeing");
            hasTargetHash = Animator.StringToHash("HasTarget");
        }

        private new void Start()
        {
            base.Start();
            excludedPlants = new List<Plant>();

            currentCell = GameManager.Instance.GridManager.GetClosestCell(transform.position);
            nextCell = currentCell;
            targetPlant = null;
            timeAtReachedPlant = 0f;
            direction = Direction.North;
            timesShooed = 0;
            isMoving = false;

            isMovingToPlant = true;
            isEating = false;
            isFleeing = false;

            transform.position = currentCell.WorldPosition;

            audioSource = GetComponent<AudioSource>();

            animator = GetComponent<Animator>();
            var behaviourFleeing = animator.GetBehaviour<InsectFleeing>();
            behaviourFleeing.insect = this;
            var behaviourSearching = animator.GetBehaviour<InsectSearching>();
            behaviourSearching.insect = this;
            var behaviourWalking = animator.GetBehaviour<InsectWalking>();
            behaviourWalking.insect = this;
            var behaviourEating = animator.GetBehaviour<InsectEating>();
            behaviourEating.insect = this;

        }

        private new void Update()
        {
            base.Update();
            UpdateAnimation();
            PlayFootstepSound();
            PlayEatingSound();
        }

        private new void OnDisable()
        {
            base.OnDisable();
            OnPlantTargeted.OnEventRaised -= ExcludePlant;
            OnPlantEaten.OnEventRaised -= ForgetPlant;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            if(currentCell)
                Gizmos.DrawSphere(currentCell.WorldPosition, 0.1f);
            if(nextCell)
                Gizmos.DrawSphere(nextCell.WorldPosition, 0.1f);

            Gizmos.color = Color.red;
            if (targetCell)
                Gizmos.DrawSphere(targetCell.WorldPosition, 0.3f);
        }

        #endregion

        #region Public Methods
        public bool SetTargetToPlant()
        {
            targetCell = GameManager.Instance.GridManager.GetRandomCellWithPlant();
            if (!targetCell) return false;
            targetPlant = (Plant)targetCell.Occupant;

            if (excludedPlants.Count > 0)
            {
                foreach (Plant excludedPlant in excludedPlants)
                {
                    if (targetPlant.Equals(excludedPlant))
                    {
                        targetCell = null;
                        targetPlant = null;
                        return false;
                    }
                }
            }

            OnPlantTargeted.RaiseEvent(targetPlant);
            return true;
        }

        public void SetFleeing()
        {
            if(!animator.GetBool(isFleeingHash))
                animator.SetBool(isFleeingHash, true);
        }

        public void IncrementShooedCount()
        {
            ++timesShooed;
            if(timesShooed == timesToResistShooing)
            {
                animator.SetBool(isFleeingHash, true);
                DebugLogger.Log(this, "Shooed Away");
            }
        }

        public void SetTargetToExit()
        {
            targetCell = GameManager.Instance.GridManager.GetClosestCell(despawnLocation);
        }
        
        public bool MoveToTarget()
        {
            if (targetCell.Equals(nextCell))
                return true;
            
            if(!isMoving)
                CalculateNextGridPosition();
            else
                MoveToNextGridPosition();

            return false;
        }
        
        public int EatPlant()
        {
            if(timeAtReachedPlant == 0f)
            {
                timeAtReachedPlant = GameManager.Instance.Time.ElapsedTime;
                DebugLogger.Log(this, "Reached target Plant");
            }

            if (targetCell.Occupant == null)
                return 0;

            if (GameManager.Instance.Time.ElapsedTime > timeAtReachedPlant + timeToEatPlant)
            {
                GameManager.Instance.GridManager.ChangeTile(targetCell.GridPosition, GroundType.FallowSoil);
                OnPlantEaten.RaiseEvent(targetPlant);
                targetPlant.RemoveFromCell();
                targetPlant = null;
                targetCell = GameManager.Instance.GridManager.GetClosestCell(despawnLocation);
                DebugLogger.Log(this, "Ate Plant");
                return 1;
            }

            return 2;
        }
        
        public void Despawn()
        {
            isFleeing = false;
            RemoveOccupantFromCells();
            Destroy(gameObject);
            DebugLogger.Log(this, "Fled to exit");
        }

        public override void Interact(Tool tool)
        {
            throw new NotImplementedException();
        }

        public override void FailedInteraction()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private Methods
        
        private void CalculateNextGridPosition()
        {
            var vectorIntToTarget = targetCell.GridPosition - currentCell.GridPosition;
            Direction randomDirection = (Direction)UnityEngine.Random.Range(0, 4);
            var nextGridPositionProbe = nextCell.GridPosition;
            switch (randomDirection)
            {
                case Direction.North:
                    if (vectorIntToTarget.y > 0)
                        nextGridPositionProbe.y += 1;
                    break;
                case Direction.South:
                    if (vectorIntToTarget.y < 0)
                        nextGridPositionProbe.y -= 1;
                    break;
                case Direction.East:
                    if (vectorIntToTarget.x > 0)
                        nextGridPositionProbe.x += 1;
                    break;
                case Direction.West:
                    if (vectorIntToTarget.x < 0)
                        nextGridPositionProbe.x -= 1;
                    break;
            }

            nextCell = GameManager.Instance.GridManager.GetGridCell(nextGridPositionProbe);
            if (nextCell.Occupant != null && nextCell != targetCell)
            {
                DebugLogger.LogUpdate(this, "Cannot move to occupied tile.");
                return;
            }
            direction = randomDirection;
            isMoving = true;
        }

        private void MoveToNextGridPosition()
        {
            transform.position = Vector3.MoveTowards(transform.position, nextCell.WorldPosition, movementSpeed * Time.deltaTime);
            if(transform.position == nextCell.WorldPosition)
            {
                isMoving = false;
                RemoveOccupantFromCells();
                AddOccupantToCells(nextCell);
                currentCell = nextCell;
            }
        }

        private void UpdateAnimation()
        {
            switch (direction)
            {
                case Direction.South:
                    insectFront.SetActive(true);
                    insectLeft.SetActive(false);
                    insectBack.SetActive(false);
                    insectRight.SetActive(false);
                    break;
                case Direction.West:
                    insectFront.SetActive(false);
                    insectLeft.SetActive(true);
                    insectBack.SetActive(false);
                    insectRight.SetActive(false);
                    break;
                case Direction.North:
                    insectFront.SetActive(false);
                    insectLeft.SetActive(false);
                    insectBack.SetActive(true);
                    insectRight.SetActive(false);
                    break;
                case Direction.East:
                    insectFront.SetActive(false);
                    insectLeft.SetActive(false);
                    insectBack.SetActive(false);
                    insectRight.SetActive(true);
                    break;
            }
        }

        private void ExcludePlant(Plant plant)
        {
            excludedPlants.Add(plant);
        }

        private void ForgetPlant(Plant plant)
        {
            if(excludedPlants.Contains(plant))
                excludedPlants.Remove(plant);
        }
        
        private void PlayEatingSound()
        {
            if (isEating && !audioSource.isPlaying)
            {
                GameManager.Instance.AudioManager.PlaySound(SoundType.sfx_insect_eating, audioSource);
            }
        }

        private void PlayFootstepSound()
        {
            if (isMoving)
            {
                if (!currentCell.GroundType.Equals(currentGroundType))
                    audioSource.Stop();
                currentGroundType = currentCell.GroundType;

                if (audioSource.isPlaying)
                    return;

                switch (currentGroundType)
                {
                    case GroundType.Grass:
                        GameManager.Instance.AudioManager.PlaySound(SoundType.sfx_insect_walking_grass, audioSource);

                        break;
                    case GroundType.Path:
                        GameManager.Instance.AudioManager.PlaySound(SoundType.sfx_insect_walking_gravel, audioSource);

                        break;
                    case GroundType.FallowSoil:
                    case GroundType.ArableSoil:
                        GameManager.Instance.AudioManager.PlaySound(SoundType.sfx_footsteps_dirt, audioSource);

                        break;
                }
            }
            else
            {
                audioSource.Stop();
            }
        }


        #endregion

    }
}
