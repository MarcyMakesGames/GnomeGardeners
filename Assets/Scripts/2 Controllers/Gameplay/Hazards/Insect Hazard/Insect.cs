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

        private GridCell currentCell, nextCell, targetCell;
        private Plant targetPlant;
        private Vector2 vectorToTarget, vectorToDespawn;
        private Vector2Int currentGridPosition, nextGridPosition, targetGridPosition;
        private float timeAtReachedPlant;
        private bool isMoving, isEating;
        private Direction direction;
        private int timesShooed;
        private GroundType currentGroundType;
        private AudioSource audioSource;

        private Vector2Int vectorIntToTarget;
        private Animator currentAnimator;

        private Animator animator;

        // State Machine (Behaviour)
        [HideInInspector]  public bool isSearchingPlant;
        [HideInInspector]  public bool isMovingToPlant;
        [HideInInspector]  public bool isFleeing;

        private int isIncomingHash;
        private int isEatingHash;
        private int isFleeingHash;
        private int hasTargetHash;

        private GridManager gridManager;

        // Event Channels
        public PlantEventChannelSO OnPlantTargeted;
        public PlantEventChannelSO OnPlantEaten;

        public GameObject AssociatedObject => gameObject;

        #region Unity Methods

        private void Awake()
        {
            gridManager = GameManager.Instance.GridManager;
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

            currentCell = gridManager.GetClosestCell(transform.position);
            nextCell = currentCell;
            targetPlant = null;
            vectorToTarget = new Vector2(0f, 0f);
            vectorToDespawn = new Vector2(0f, 0f);
            currentGridPosition = currentCell.GridPosition;
            nextGridPosition = nextCell.GridPosition;
            targetGridPosition = new Vector2Int(0, 0);
            timeAtReachedPlant = 0f;
            direction = Direction.North;
            timesShooed = 0;
            isMoving = false;

            isMovingToPlant = true;
            isEating = false;
            isFleeing = false;

            transform.position = currentCell.WorldPosition;

            audioSource = GetComponent<AudioSource>();
        }

        private new void Update()
        {
            base.Update();
            if (isSearchingPlant)
            {
                SetTargetToPlant();
            }
            else if(isMovingToPlant)
            {
                MoveToTarget(targetCell);

            }
            else if(isFleeing)
            {
                MoveToTarget(targetCell);
            }

            UpdateAnimation();
            PlayFootstepSound();
            PlayEatingSound();
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
            targetCell = gridManager.GetRandomCellWithPlant();
            if (!targetCell) return false;
            targetPlant = (Plant)targetCell.Occupant;
            targetGridPosition = targetCell.GridPosition;

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

            isSearchingPlant = false;
            isMovingToPlant = true;
            DebugLogger.Log(this, "Found target Plant");
            return true;
        }

        public void SetFleeing()
        {
            animator.SetBool(isFleeingHash, true);
        }

        public void IncrementShooedCount()
        {
            ++timesShooed;
            if(timesShooed == timesToResistShooing)
            {
                SetFleeing();
                DebugLogger.Log(this, "Shooed Away");
            }
        }

        public void SetTargetToExit()
        {
            targetCell = gridManager.GetClosestCell(despawnLocation);
            targetGridPosition = targetCell.GridPosition;
        }

        public void MoveToTarget()
        {
            vectorToTarget = targetCell.WorldPosition - transform.position;

            if (!isMoving)
            {
                CalculateNextGridPosition();
            }

            if (targetCell.Equals(nextCell))
            {
                if (animator.GetBool(isFleeingHash))
                {
                    animator.SetBool(isFleeingHash, false);
                }
                else if (animator.GetBool(isIncomingHash))
                {
                    animator.SetBool(isIncomingHash, false);
                    animator.SetBool(isEatingHash, true);
                }
            }
            else
            {
                MoveToNextGridPosition();
            }
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

        private void MoveToTarget(GridCell targetCell)
        {
            vectorToTarget = targetCell.WorldPosition - transform.position;

            if (!isMoving)
            {
                CalculateNextGridPosition();
            }

            if(targetCell.Equals(nextCell))
            {
                if (animator.GetBool(isFleeingHash))
                {
                    animator.SetBool(isFleeingHash, false);
                }
                else if (animator.GetBool(isIncomingHash))
                {
                    EatPlant();
                }
            }
            else
            {
                MoveToNextGridPosition();
            }
        }

        private void CalculateNextGridPosition()
        {
            vectorIntToTarget = targetGridPosition - currentGridPosition;
            Direction randomDirection = (Direction)UnityEngine.Random.Range(0, 4);
            var nextGridPositionProbe = nextGridPosition;
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

            nextCell = gridManager.GetGridCell(nextGridPositionProbe);
            if (nextCell.Occupant != null)
            {
                DebugLogger.LogUpdate(this, "Cannot move to occupied tile.");
                return;
            }
            direction = randomDirection;
            nextGridPosition = nextGridPositionProbe;
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
                currentGridPosition = currentCell.GridPosition;
            }
        }



        private void EatPlant()
        {
            if(timeAtReachedPlant == 0f)
            {
                timeAtReachedPlant = GameManager.Instance.Time.ElapsedTime;
                isEating = true;
                DebugLogger.Log(this, "Reached target Plant");
            }

            if (GameManager.Instance.Time.ElapsedTime > timeAtReachedPlant + timeToEatPlant)
            {
                gridManager.ChangeTile(targetCell.GridPosition, GroundType.FallowSoil);
                OnPlantEaten.RaiseEvent(targetPlant);
                targetPlant.RemoveFromCell();
                targetPlant = null;
                isSearchingPlant = false;
                isMovingToPlant = false;
                isFleeing = true;
                targetCell = gridManager.GetClosestCell(despawnLocation);
                targetGridPosition = targetCell.GridPosition;
                isEating = false;
                DebugLogger.Log(this, "Ate Plant");
            }
        }
        private void UpdateAnimation()
        {
            if (direction == Direction.South)
            {
                insectFront.SetActive(true);
                insectLeft.SetActive(false);
                insectBack.SetActive(false);
                insectRight.SetActive(false);
                currentAnimator = insectFront.GetComponent<Animator>();
            }
            else if (direction == Direction.West)
            {
                insectFront.SetActive(false);
                insectLeft.SetActive(true);
                insectBack.SetActive(false);
                insectRight.SetActive(false);
                currentAnimator = insectLeft.GetComponent<Animator>();
            }
            else if (direction == Direction.North)
            {
                insectFront.SetActive(false);
                insectLeft.SetActive(false);
                insectBack.SetActive(true);
                insectRight.SetActive(false);
                currentAnimator = insectBack.GetComponent<Animator>();
            }
            else if (direction == Direction.East)
            {
                insectFront.SetActive(false);
                insectLeft.SetActive(false);
                insectBack.SetActive(false);
                insectRight.SetActive(true);
                currentAnimator = insectRight.GetComponent<Animator>();
            }

            currentAnimator.SetBool("IsEating", isEating);
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


        #endregion

    }
}
