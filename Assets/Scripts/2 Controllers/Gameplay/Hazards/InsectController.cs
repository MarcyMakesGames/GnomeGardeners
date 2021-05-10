using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsectController : MonoBehaviour
{
    [HideInInspector] public Vector3 despawnLocation;

    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float timeToEatPlant = 5f;
    [SerializeField] private int timesToResistShooing = 3;
    [SerializeField] private float rangeToEat = 2f;

    private Vector2 velocity;
    private Vector2 vectorToTarget;
    private GridCell targetCell;
    private Plant targetPlant;
    private float timeAtReachedPlant;
    private Vector2 vectorToDespawn;
    private bool isSearchingPlant;

    private GridManager gridManager;

    #region Unity Methods

    private void Awake()
    {
        gridManager = GameManager.Instance.GridManager;
    }

    private void Start()
    {
        FindTargetPlant();
    }

    private void FixedUpdate()
    {
        if (isSearchingPlant)
        {
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
            Flee();
        }
    }

    #endregion

    #region Private Methods

    private void MoveToPlant()
    {
        if(targetPlant == null) { FindTargetPlant(); }

        vectorToTarget = targetCell.WorldPosition - transform.position;

        transform.Translate(vectorToTarget * movementSpeed);
    }

    private void FindTargetPlant()
    {
        targetCell = gridManager.GetRandomCellWithPlant();
        targetPlant = (Plant)targetCell.Occupant;
    }

    private void EatPlant()
    {
        if (targetPlant == null) { FindTargetPlant(); }

        timeAtReachedPlant = GameManager.Instance.Time.ElapsedTime;
        if(GameManager.Instance.Time.ElapsedTime > timeAtReachedPlant + timeToEatPlant)
        {
            Flee();
            gridManager.ChangeTile(targetCell.GridPosition, GroundType.FallowSoil);
            gridManager.ChangeTileOccupant(targetCell.GridPosition, null);
            Destroy(targetPlant.gameObject);
        }
    }

    private void Flee()
    {
        if(despawnLocation == null) { return; }

        vectorToDespawn = despawnLocation - transform.position;

        transform.Translate(vectorToDespawn * movementSpeed);
    }

    #endregion

}
