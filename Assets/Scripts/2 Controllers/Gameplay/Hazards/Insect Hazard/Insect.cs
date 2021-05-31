using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        private Animator animatorFront;
        private Animator animatorBack;
        private Animator animatorLeft;
        private Animator animatorRight;

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

        // State Machine
        private State currentState = State.SearchingPlant;
        private enum State
        {
            SearchingPlant,
            MovingToPlant,
            Eating,
            Fleeing,
        }

        private GridManager gridManager;

        // Event Channels
        public PlantEventChannelSO OnPlantTargeted;
        public PlantEventChannelSO OnPlantEaten;

        public GameObject AssociatedObject => gameObject;

        public bool IsEating => isEating;

        #region Unity Methods

        private void Awake()
        {
            gridManager = GameManager.Instance.GridManager;
            OnPlantTargeted.OnEventRaised += ExcludePlant;
            OnPlantEaten.OnEventRaised += ForgetPlant;
        }

        private new void Start()
        {
            base.Start();
            excludedPlants = new List<Plant>();
            currentCell = gridManager.GetClosestCell(transform.position);
            nextCell = currentCell;
            targetPlant = null;
            timeAtReachedPlant = 0f;
            isMoving = false;
            isEating = false;
            direction = Direction.North;
            timesShooed = 0;
            transform.position = currentCell.WorldPosition;
            audioSource = GetComponent<AudioSource>();
            animatorFront = insectFront.GetComponent<Animator>();
            animatorBack = insectBack.GetComponent<Animator>();
            animatorLeft = insectLeft.GetComponent<Animator>();
            animatorRight = insectRight.GetComponent<Animator>();
        }

        private new void Update()
        {
            base.Update();
            
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

            animatorFront.SetBool("IsEating", isEating);
            animatorBack.SetBool("IsEating", isEating);
            animatorLeft.SetBool("IsEating", isEating);
            animatorRight.SetBool("IsEating", isEating);
            PlayFootstepSound();
            PlayEatingSound();
        }

        private void FixedUpdate()
        {
            switch (currentState)
            {
                case State.SearchingPlant:
                    targetCell = gridManager.GetRandomCellWithPlant();
                    if(targetCell)
                    {
                        targetPlant = (Plant)targetCell.Occupant;
                        if(excludedPlants.Count > 0)
                        {
                            if (Enumerable.Contains(excludedPlants, targetPlant))
                            {
                                targetCell = null;
                                targetPlant = null;
                                break;
                            }
                        }
                        OnPlantTargeted.RaiseEvent(targetPlant);
                        currentState = State.MovingToPlant;
                    }
                    break;
                
                case State.MovingToPlant:
                    CalculateNextMove();

                    if (targetCell.Equals(nextCell))
                    {
                        currentState = State.Eating;
                        break;
                    }
                    
                    Move();
                    break;
                
                case State.Eating:
                    // insect has started eating
                    if (!isEating)
                    {
                        timeAtReachedPlant = GameManager.Instance.Time.ElapsedTime;
                        isEating = true;
                    }

                    // plant disappears while insect is eating
                    if (!targetCell.Occupant)
                    {
                        currentState = State.SearchingPlant;
                        isEating = false;
                    }

                    // insect has finished eating
                    if (GameManager.Instance.Time.ElapsedTime > timeAtReachedPlant + timeToEatPlant)
                    {
                        gridManager.ChangeTile(targetCell.GridPosition, GroundType.FallowSoil);
                        OnPlantEaten.RaiseEvent(targetPlant);
                        targetPlant.RemoveFromCell();
                        targetPlant = null;
                        isEating = false;
                        currentState = State.Fleeing;
                    }

                    break;
                case State.Fleeing:
                    targetCell = gridManager.GetClosestCell(despawnLocation);
                    CalculateNextMove();
                    if(targetCell.Equals(nextCell))
                    {
                        RemoveOccupantFromCells();
                        Destroy(gameObject);
                    }
                    Move();
                    
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Move()
        {
            transform.position =
                Vector3.MoveTowards(transform.position, nextCell.WorldPosition, movementSpeed * Time.deltaTime);
            if (transform.position == nextCell.WorldPosition && nextCell.Occupant == null)
            {
                isMoving = false;
                RemoveOccupantFromCells();
                AddOccupantToCells(nextCell);
                currentCell = nextCell;
            }
        }

        private new void OnDisable()
        {
            base.OnDisable();
            OnPlantTargeted.OnEventRaised -= ExcludePlant;
            OnPlantEaten.OnEventRaised -= ForgetPlant;
        }

        #endregion

        #region Public Methods

        public void SetFleeing()
        {
            isEating = false;
            currentState = State.Fleeing;
            targetCell = gridManager.GetClosestCell(despawnLocation);
        }

        public void IncrementShooedCount()
        {
            ++timesShooed;
            if(timesShooed == timesToResistShooing)
                SetFleeing();
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
        
        private void CalculateNextMove()
        {
            if (!isMoving)
            {
                var vectorIntToTarget = targetCell.GridPosition - currentCell.GridPosition;
                Direction randomDirection = (Direction) UnityEngine.Random.Range(0, 4);
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

                nextCell = gridManager.GetGridCell(nextGridPositionProbe);
                if (!nextCell.Occupant)
                {
                    direction = randomDirection;
                    isMoving = true;
                }
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
