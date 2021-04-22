using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour, IInteractable, IHoldable, IOccupant
{
    public static bool DEBUG = false;

    [SerializeField] private Species species;
    private Stage currentStage;
    private SpriteRenderer plantRenderer;
    private float currentGrowTime;
    private bool isOnArableGround;

    private bool isBeingCarried;

    bool IHoldable.IsBeingCarried { get => isBeingCarried; set => isBeingCarried = value; }

    public GameObject GameObject => gameObject;

    public Stage CurrentStage { get => currentStage; }

    #region Unity Methods
#if DEBUG
    [ContextMenu("SatisfyCurrentNeed")]
    private void SatisfyCurrentNeed()
    {
        currentStage.SatisfyNeed(50f);
    }
#endif
    private void Awake()
    {
        plantRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        Initialize();
        Log("Debugging");
    }

    private void Update()
    {
        Grow();
    }

    #endregion

    #region Public Methods
    public void Interact(Tool tool = null)
    {
    }

    public void HarvestPlant()
    {
        // harvest this plant
        throw new NotImplementedException();
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
    }

    public void Drop(Vector2 position)
    {
        gameObject.SetActive(true);
        transform.position = position;
        CheckArableGround(position);
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
    private void Grow()
    {
        if (!isOnArableGround)
            return;

        if (GameManager.Instance.Time.GetTimeSince(currentGrowTime) >= currentStage.TimeToNextStage)
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
        currentStage = species.NextStage(currentStage.Index);
        currentGrowTime = GameManager.Instance.Time.ElapsedTime;
        plantRenderer.sprite = currentStage.sprite;
        name = currentStage.name + " " + species.name;
    }

    private void CheckArableGround(Vector3 checkPosition)
    {
        // to-do: check tile through grid manager

        //Vector3 localDirection = new Vector3(transform.position.x, -1f, transform.position.z);
        //Vector3 direction = transform.TransformDirection(localDirection);
        //Ray ray = new Ray(transform.position, direction);
        //RaycastHit hit;
        //Debug.DrawRay(transform.position, direction);


        //if (Physics.Raycast(ray, out hit))
        //{
        //    if (hit.collider.GetComponent<Ground>().type == GroundType.ArableSoil)
        //        isOnArableGround = true;
        //    else
        //        isOnArableGround = false;
        //}
    }

    private void Initialize()
    {
        if (species.stages.Count > 0)
            currentStage = species.stages[0];
        else
            Debug.LogWarning("Plant.cs: Species not selected or set-up.");
        isOnArableGround = true;
        plantRenderer.sprite = currentStage.sprite;
        name = currentStage.name+" "+species.name;
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
