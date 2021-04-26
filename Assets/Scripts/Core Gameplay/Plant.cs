using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour, IInteractable, IHoldable, IOccupant
{
    public bool debug = false;

    public Species species;

    private Stage currentStage;
    private float lastStageTimeStamp;
    private float currentGrowTime;
    private bool isOnArableGround;
    private bool isBeingCarried;
    private GridCell occupyingCell;

    public bool IsBeingCarried { get => isBeingCarried; set => isBeingCarried = value; }
    public Stage CurrentStage { get => currentStage; }
    public GameObject AssociatedObject { get => gameObject; }

    public SpriteRenderer spriteRenderer;

    public VoidEventChannelSO OnTileChanged;

    #region Unity Methods

    private void Start()
    {
        Configure();
        Log("Debugging");
    }

    private void Update()
    {
        TryGrowing();
    }

    public void OnValidate()
    {
        if(species != null)
        {
            currentStage = species.stages[0];
            spriteRenderer.sprite = currentStage.sprite;
        }
        else
        {
            LogWarning("A plant requires a species.");
        }
    }

    private void OnDisable()
    {
        Dispose();
    }


    #endregion

    #region Public Methods
    public void Interact(Tool tool = null)
    {
    }

    public void HarvestPlant()
    {
        // todo: object pool stash
        if (currentStage.isHarvestable)
        {
            gameObject.SetActive(false);
            Dispose();
        }
    }

    public void WaterPlant(float amount)
    {
        currentStage.SatisfyNeed(NeedType.Water, amount);
    }

    public void PlantSeed(GridCell cell)
    {
        if (currentStage.specifier != PlantStage.Seed)
            return;

        gameObject.SetActive(true);
        transform.position = cell.WorldPosition;
        cell.Occupant = this;
        occupyingCell = cell;

        CheckArableGround();
    }

    public void Drop(Vector2 position)
    {
        gameObject.SetActive(true);
        transform.position = position;
        Debug.Log("Planted at " + transform.position);
    }

    public void Hold()
    {
        throw new NotImplementedException();
    }
    #endregion

    #region Private Methods
    private void TryGrowing()
    {
        if (!isOnArableGround)
            return;

        currentGrowTime = GameManager.Instance.Time.GetTimeSince(lastStageTimeStamp) * species.growMultiplier;

        if ( currentGrowTime >= currentStage.timeToNextStage)
        {
            AdvanceStages();
        }
    }

    private void AdvanceStages()
    {
        var stageIsReady = currentStage.IsReady();
        if (!stageIsReady)
        {
            return;
        }
        currentStage = species.NextStage(currentStage.index);
        Log("Grew into stage: " + currentStage.specifier.ToString());
        lastStageTimeStamp = GameManager.Instance.Time.ElapsedTime;
        spriteRenderer.sprite = currentStage.sprite;
        name = currentStage.name + " " + species.name;
    }

    private void CheckArableGround()
    {
        if(occupyingCell.GroundType == GroundType.ArableSoil)
        {
            Log("Found Arable Ground!");
            isOnArableGround = true;
        }
        else
        {
            isOnArableGround = false;
        }
    }

    private void CheckOccupyingCell()
    {
        occupyingCell = GameManager.Instance.GridManager.GetClosestCell(transform.position);
    }

    private void Configure()
    {
        if (species.stages.Count > 0)
            currentStage = species.stages[0];
        else
            LogWarning("Plant.cs: Species not selected or set-up.");
        isOnArableGround = false;
        spriteRenderer.sprite = currentStage.sprite;
        name = currentStage.name+" "+species.name;

        OnTileChanged.OnEventRaised += CheckOccupyingCell;
        OnTileChanged.OnEventRaised += CheckArableGround;
    }

    private void Dispose()
    {
        spriteRenderer.sprite = null;
        name = species.name;
        currentStage = null;

        OnTileChanged.OnEventRaised -= CheckOccupyingCell;
        OnTileChanged.OnEventRaised -= CheckArableGround;
    }

    private void Log(string msg)
    {
        if (!debug) { return; }
        Debug.Log("[Plant]: " + msg);
    }

    private void LogWarning(string msg)
    {
        if (!debug) { return; }
        Debug.LogWarning("[Plant]: " + msg);
    }

    #endregion
}
