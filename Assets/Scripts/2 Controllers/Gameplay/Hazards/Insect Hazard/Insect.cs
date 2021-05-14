using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Insect : MonoBehaviour, IOccupant
{
    private bool debug = false;

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

    private Vector2Int vectorIntToTarget;
    private Animator currentAnimator;

    // State Machine (Behaviour)
    [HideInInspector]  public bool isSearchingPlant;
    [HideInInspector]  public bool isMovingToPlant;
    [HideInInspector]  public bool isFleeing;

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
    }

    private void Start()
    {
        
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
        isMoving = false;
        isEating = false;
        direction = Direction.North;
        timesShooed = 0;

        isSearchingPlant = true;
        isMovingToPlant = false;
        isFleeing = false;

        transform.position = currentCell.WorldPosition;
        AssignOccupant();
    }

    private void Update()
    {
        if (isSearchingPlant)
        {
            FindTargetPlant();
        }
        else if(isMovingToPlant)
        {
            if(timesShooed >= timesToResistShooing)
            {
                SetFleeing();
            }
            MoveToTarget(targetCell);

        }
        else if(isFleeing)
        {
            MoveToTarget(targetCell);
        }

        UpdateAnimation();
    }



    private void OnDisable()
    {
        OnPlantTargeted.OnEventRaised -= ExcludePlant;
        OnPlantEaten.OnEventRaised -= ForgetPlant;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(currentCell.WorldPosition, 0.1f);
        Gizmos.DrawSphere(nextCell.WorldPosition, 0.1f);
        if(targetCell)
            Gizmos.DrawSphere(targetCell.WorldPosition, 0.1f);
    }

    #endregion

    #region Public Methods

    public void AssignOccupant()
    {
        gridManager.ChangeTileOccupant(gridManager.GetClosestGrid(transform.position), this);
    }

    public void SetFleeing()
    {
        isFleeing = true;
        isMovingToPlant = false;
        isSearchingPlant = false;
        targetCell = gridManager.GetClosestCell(despawnLocation);
    }

    public void IncrementShooedCount()
    {
        ++timesShooed;
        if(timesShooed >= timesToResistShooing)
        {
            currentCell.RemoveCellOccupant();
            Destroy(gameObject);
            Log("Shooed Away");
        }
    }

    #endregion

    #region Private Methods

    private void FindTargetPlant()
    {
        if(targetPlant != null) { return; }
        targetCell = gridManager.GetRandomCellWithPlant();
        if(targetCell == null) { return; }
        targetPlant = (Plant)targetCell.Occupant;
        targetGridPosition = targetCell.GridPosition;

        if(excludedPlants.Count > 0)
        {
            foreach(Plant excludedPlant in excludedPlants)
            {
                if (targetPlant.Equals(excludedPlant))
                {
                    targetCell = null;
                    targetPlant = null;
                    return;
                }
            }
        }
        OnPlantTargeted.RaiseEvent(targetPlant);

        isSearchingPlant = false;
        isMovingToPlant = true;
        Log("Found target Plant");
    }

    private void MoveToTarget(GridCell targetCell)
    {
        vectorToTarget = targetCell.WorldPosition - transform.position;

        if (!isMoving)
        {
            CalculateNextGridPosition();
        }

        if(targetCell.Equals(nextCell))
        {
            if (isFleeing)
            {
                DespawnAtTargetCell();
            }
            else if (isMovingToPlant)
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
            Log("Cannot move to occupied tile.");
            return;
        }
        direction = randomDirection;
        nextGridPosition = nextGridPositionProbe;
        isMoving = true;
    }

    private void MoveToNextGridPosition( )
    {
        transform.position = Vector3.MoveTowards(transform.position, nextCell.WorldPosition, movementSpeed * Time.deltaTime);
        if(transform.position == nextCell.WorldPosition)
        {
            isMoving = false;
            currentCell.RemoveCellOccupant();
            nextCell.AddCellOccupant(this);
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
            Log("Reached target Plant");
        }

        if (GameManager.Instance.Time.ElapsedTime > timeAtReachedPlant + timeToEatPlant)
        {
            gridManager.ChangeTile(targetCell.GridPosition, GroundType.FallowSoil);
            gridManager.ChangeTileOccupant(targetCell.GridPosition, null);
            OnPlantEaten.RaiseEvent(targetPlant);
            Destroy(targetPlant.gameObject);
            targetPlant = null;
            isSearchingPlant = false;
            isMovingToPlant = false;
            isFleeing = true;
            targetCell = gridManager.GetClosestCell(despawnLocation);
            targetGridPosition = targetCell.GridPosition;
            isEating = false;
            Log("Ate Plant");
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

    private void DespawnAtTargetCell()
    {

        isFleeing = false;
        // to-do: replace with object pool
        currentCell.RemoveCellOccupant();
        Destroy(gameObject);
        Log("Fled to exit");

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

    private void Log(string msg)
    {
        if (!debug) { return; }
        Debug.Log("[Insect]: " + msg);
    }

    private void LogWarning(string msg)
    {
        if (!debug) { return; }
        Debug.LogWarning("[Insect]: " + msg);
    }

    #endregion

}
