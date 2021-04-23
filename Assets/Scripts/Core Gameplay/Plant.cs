using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour, IInteractable, IHoldable, IOccupant
{
    public static bool DEBUG = false;

    public Species species;

    private Stage currentStage;
    private float currentGrowTime;
    private bool isOnArableGround;
    private bool isBeingCarried;
    private GridCell occupyingCell;

    bool IHoldable.IsBeingCarried { get => isBeingCarried; set => isBeingCarried = value; }

    public GameObject GameObject => gameObject;

    public Stage CurrentStage { get => currentStage; }

    public SpriteRenderer spriteRenderer;

    #region Unity Methods

#if DEBUG

    [ContextMenu("SatisfyCurrentNeed")]
    private void SatisfyAllNeeds()
    {
        currentStage.SatisfyNeeds(50f);
    }

#endif

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

    #endregion

    #region Public Methods
    public void Interact(Tool tool = null)
    {
    }

    public void HarvestPlant()
    {
        // todo: object pool stash
        gameObject.SetActive(false);
        Dispose();
    }

    public void WaterPlant()
    {
        // water this plant
        throw new NotImplementedException();
    }

    public void PlantSeed(GridCell cell)
    {
        if (currentStage.specifier != PlantStage.Seed)
            return;

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

    public void IsBeingCarried()
    {
        throw new NotImplementedException();
    }
    #endregion

    #region Private Methods
    private void TryGrowing()
    {
        if (!isOnArableGround)
            return;

        if (GameManager.Instance.Time.GetTimeSince(currentGrowTime) >= currentStage.timeToNextStage)
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
        currentGrowTime = GameManager.Instance.Time.ElapsedTime;
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

    private void Configure()
    {
        if (species.stages.Count > 0)
            currentStage = species.stages[0];
        else
            LogWarning("Plant.cs: Species not selected or set-up.");
        isOnArableGround = false;
        spriteRenderer.sprite = currentStage.sprite;
        name = currentStage.name+" "+species.name;
    }

    private void Dispose()
    {
        spriteRenderer.sprite = null;
        name = species.name;
        currentStage = null;
    }

    private void Log(string msg)
    {
        if (!DEBUG) { return; }
        Debug.Log("[Plant]: " + msg);
    }

    private void LogWarning(string msg)
    {
        if (!DEBUG) { return; }
        Debug.LogWarning("[Plant]: " + msg);
    }

    #endregion
}
