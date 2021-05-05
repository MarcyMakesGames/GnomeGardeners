using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour, IInteractable, IHoldable
{
    public bool debug = true;

    public Species species;
    private Sprite spriteInHand;

    private Stage currentStage;
    private float lastStageTimeStamp;
    private float currentGrowTime;
    private bool isOnArableGround;
    private bool isBeingCarried;
    private bool isDecayed;
    private GridCell occupyingCell;
    private float currentNeedValue;
    private bool isNeedFulfilled;

    public bool IsBeingCarried { get => isBeingCarried; set => isBeingCarried = value; }
    public Stage CurrentStage { get => currentStage; }
    public GameObject AssociatedObject { get => gameObject; }
    public Sprite SpriteInHand { get => spriteInHand; set => spriteInHand = value; }

    public SpriteRenderer spriteRenderer;

    public VoidEventChannelSO OnTileChanged;

    #region Unity Methods

    private void Start()
    {
        Configure();
        Log("Debugging");
        AssignOccupant();
    }

    private void Update()
    {
        TryGrowing();
    }

    private void OnValidate()
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

    void OnDestroy()
    {
        Dispose();
    }


    #endregion

    #region Public Methods
    public void Interact(Tool tool = null)
    {
    }

    public void HarvestPlant(GridCell cell)
    {
        // todo: object pool stash
        if (currentStage.isHarvestable)
        {
            cell.RemoveCellOccupant();
            gameObject.SetActive(false);
            isBeingCarried = true;
        }
    }

    public void AddToNeedValue(float amount)
    {
        Log("Adding" + amount.ToString() + " to need value.");
        currentNeedValue += amount;
        if(currentNeedValue >= currentStage.need.threshold)
        {
            isNeedFulfilled = true;
        }
    }

    public void PlantSeed(GridCell cell)
    {
        if (currentStage.specifier != PlantStage.Seed)
            return;

        transform.position = cell.WorldPosition;
        cell.Occupant = this;
        occupyingCell = cell;
        isBeingCarried = false;

        CheckArableGround();
    }

    public void AssignOccupant()
    {
        GameManager.Instance.GridManager.ChangeTileOccupant(GameManager.Instance.GridManager.GetClosestGrid(AssociatedObject.transform.position), this);
    }
    #endregion

    #region Private Methods
    private void TryGrowing()
    {
        if (isDecayed) { return; }

        if (!isOnArableGround) { return; }

        LogUpdate("Tries Growing on arable ground.");
        currentGrowTime = GameManager.Instance.Time.GetTimeSince(lastStageTimeStamp) * species.growMultiplier;

        if ( currentGrowTime >= currentStage.timeToNextStage)
        {
            if (isNeedFulfilled)
                AdvanceStages();
            else
                AdvanceToDecayedStage();
        }
    }

    private void AdvanceToDecayedStage()
    {
        Log("Grew into decayed stage.");
        currentStage = species.decayedStage;
        lastStageTimeStamp = GameManager.Instance.Time.ElapsedTime;
        spriteRenderer.sprite = species.decayedStage.sprite;
        name = "Decayed" + species.name;
        currentNeedValue = 0f;
        isNeedFulfilled = false;
        isDecayed = true;
        GameManager.Instance.GridManager.ChangeTile(occupyingCell.GridPosition, GroundType.FallowSoil);
        spriteInHand = species.deadSprite;
    }

    private void AdvanceStages()
    {
        Log("Grew into stage: " + currentStage.specifier.ToString());
        var currentStageIndex = species.stages.IndexOf(currentStage);
        currentStage = species.NextStage(currentStageIndex);
        lastStageTimeStamp = GameManager.Instance.Time.ElapsedTime;
        spriteRenderer.sprite = currentStage.sprite;
        name = currentStage.name + " " + species.name;
        currentNeedValue = 0f;
        isNeedFulfilled = false;
        if(currentStage.specifier == PlantStage.Ripening) { spriteInHand = species.harvestSprite; }
    }

    private void CheckArableGround()
    {
        if(occupyingCell.GroundType == GroundType.ArableSoil)
        {
            Log("Found Arable Ground!");
            isOnArableGround = true;
            lastStageTimeStamp = GameManager.Instance.Time.ElapsedTime;
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
        currentNeedValue = 0f;
        isBeingCarried = true;
        spriteInHand = species.prematureSprite;

        OnTileChanged.OnEventRaised += CheckOccupyingCell;
        OnTileChanged.OnEventRaised += CheckArableGround;
    }

    private void Dispose()
    {
        Log("Being disposed of.");
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

    private void LogUpdate(string msg)
    {
        if (!debug) { return; }
        if(Time.time % 3f <= Time.deltaTime)
        {
            Debug.Log("[Plant]: " + msg);
        }
    }


    #endregion
}