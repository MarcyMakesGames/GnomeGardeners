using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Insect : MonoBehaviour
{
    private bool debug = true;

    [HideInInspector] public Vector3 despawnLocation = new Vector3(0f, 0f, 0f);

    [SerializeField] private float movementSpeed = 0.1f;
    [SerializeField] private float timeToEatPlant = 5f;
    [SerializeField] private int timesToResistShooing = 3;
    [SerializeField] private float rangeToEat = 1f;

    private Vector2 velocity;
    private Vector2 vectorToTarget;
    private GridCell targetCell;
    private Plant targetPlant;
    private float timeAtReachedPlant;
    private Vector2 vectorToDespawn;
    private bool isSearchingPlant;
    private bool isMovingToPlant;
    private bool isFleeing;

    private GridManager gridManager;

    #region Unity Methods

    private void Awake()
    {
        gridManager = GameManager.Instance.GridManager;
    }

    private void Start()
    {
        isSearchingPlant = true;
        isMovingToPlant = false;
        isFleeing = false;
        FindTargetPlant();
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

    #endregion

    #region Private Methods

    private void FindTargetPlant()
    {
        if(targetPlant != null) { return; }
        targetCell = gridManager.GetRandomCellWithPlant();
        if(targetCell == null) { return; }
        targetPlant = (Plant)targetCell.Occupant;
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
        transform.Translate(vectorToTarget * movementSpeed * Time.deltaTime);
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

        transform.Translate(vectorToDespawn * movementSpeed * Time.deltaTime);

        if(vectorToDespawn.magnitude < 1f)
        {
            isFleeing = false;
        }

        Log("Fled to exit");
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
