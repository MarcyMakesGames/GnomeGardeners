using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Insect : MonoBehaviour, IOccupant
{
    private bool debug = true;

    [HideInInspector] public Vector3 despawnLocation = new Vector3(0f, 0f, 0f);
    [HideInInspector] public List<Plant> excludedPlants;

    [SerializeField] private float movementSpeed = 0.1f;
    [SerializeField] private float timeToEatPlant = 5f;
    [SerializeField] private int timesToResistShooing = 3;
    [SerializeField] private float rangeToEat = 1f;

    private Vector2 velocity, vectorToTarget, vectorToNextCell, vectorToDespawn;
    private Vector2Int nextGridPosition;
    private GridCell currentCell, previousCell, nextCell, targetCell;
    private Plant targetPlant;
    private float timeAtReachedPlant;

    // State Machine (Behaviour)
    public bool isSearchingPlant;
    public bool isMovingToPlant;
    public bool isFleeing;
    private bool isMoving;

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
        isSearchingPlant = true;
        isMovingToPlant = false;
        isFleeing = false;
        isMoving = false;
        excludedPlants = new List<Plant>();
        FindTargetPlant();
        transform.position = gridManager.GetClosestCell(transform.position).WorldPosition;
        nextGridPosition = currentCell.GridPosition;
    }

    private void Update()
    {
        if (isSearchingPlant)
        {
            FindTargetPlant();
        }
        else if(isMovingToPlant)
        {
            CalculateVectorToTarget();

            if (vectorToTarget.magnitude > rangeToEat)
            {
                MoveToPlant();
            }
            else
            {
                EatPlant();
            }
        }
        else
        {
            if (isFleeing)
            {
                Flee();
            }
        }
    }

    private void OnDisable()
    {
        OnPlantTargeted.OnEventRaised -= ExcludePlant;
        OnPlantEaten.OnEventRaised -= ForgetPlant;
    }

    #endregion

    #region Public Methods

    public void AssignOccupant()
    {
        gridManager.ChangeTileOccupant(gridManager.GetClosestGrid(transform.position), this);
    }

    #endregion

    #region Private Methods

    private void FindTargetPlant()
    {
        if(targetPlant != null) { return; }
        targetCell = gridManager.GetRandomCellWithPlant();
        if(targetCell == null) { return; }
        targetPlant = (Plant)targetCell.Occupant;

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

    private void CalculateVectorToTarget()
    {
        if(targetPlant == null) { return; }

        vectorToTarget = targetCell.WorldPosition - transform.position;
    }

    private void MoveToPlant()
    {
        if (!isMoving)
        {
            CalculateNextGridPosition();
        }
        else
        {
            MoveToGridPosition(nextGridPosition);
        }

    }

    private void CalculateNextGridPosition()
    {
        var vectorIntToTarget = targetCell.GridPosition - currentCell.GridPosition;
        var moveHorizontally = UnityEngine.Random.Range(0, 1);
        if (moveHorizontally == 0)
        {
            if (vectorIntToTarget.x > 0f)
            {
                nextGridPosition.x += 1;
            }
            else
            {
                nextGridPosition.x -= 1;
            }
        }
        else
        {
            if (vectorIntToTarget.y > 0f)
            {
                nextGridPosition.y += 1;
            }
            else
            {
                nextGridPosition.y -= 1;
            }
        }

        nextCell = gridManager.GetGridCell(nextGridPosition);
        if (nextCell.Occupant != null)
        {
            Log("Cannot move to occupied tile.");
            return;
        }
        isMoving = true;
    }

    private void MoveToGridPosition(Vector2Int gridPosition)
    {
        var direction = nextCell.WorldPosition - transform.position;
        velocity = direction * movementSpeed;
        transform.Translate(velocity * Time.deltaTime);
        vectorToNextCell = nextCell.WorldPosition - transform.position;
        if(vectorToNextCell.magnitude < movementSpeed)
        {
            isMoving = false;
        }
    }


    private void EatPlant()
    {
        if (targetPlant == null) { FindTargetPlant(); }

        if(timeAtReachedPlant == 0f)
        {
            timeAtReachedPlant = GameManager.Instance.Time.ElapsedTime;
            Log("Reached target Plant");
        }

        if (GameManager.Instance.Time.ElapsedTime > timeAtReachedPlant + timeToEatPlant)
        {
            gridManager.ChangeTile(targetCell.GridPosition, GroundType.FallowSoil);
            gridManager.ChangeTileOccupant(targetCell.GridPosition, null);
            OnPlantEaten.RaiseEvent(targetPlant);
            Destroy(targetPlant.gameObject);
            Log("Ate Plant");
            isSearchingPlant = false;
            isFleeing = true;
        }
    }

    private void Flee()
    {
        if(despawnLocation == null) { return; }

        vectorToDespawn = despawnLocation - transform.position;

        velocity = vectorToDespawn.normalized * movementSpeed;

        transform.Translate(velocity * Time.deltaTime);

        if(vectorToDespawn.magnitude < 1f)
        {
            isFleeing = false;
            // to-do: replace with object pool
            Destroy(gameObject);
            Log("Fled to exit");
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
