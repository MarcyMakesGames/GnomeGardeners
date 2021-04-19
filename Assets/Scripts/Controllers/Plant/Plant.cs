using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour, IInteractable, IHoldable
{
    public static bool DEBUG = true;

    [SerializeField] private Species species;
    private Stage currentStage;
    private SpriteRenderer plantRenderer;
    private new string name;
    private float currentGrowTime;
    private bool isOnArableGround;

    public string Name { get; }

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
    }

    private void Update()
    {
        Grow();
    }

    #endregion

    #region Public Methods
    public void Interact(ITool tool = null)
    {
        // todo: do behaviour based on tool and plant stage.
        //I.e. watering can + any stage, add to moisture
        //reaping tool + any stage returns an appropriate harvest
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

        Vector3 localDirection = new Vector3(transform.position.x, -1f, transform.position.z);
        Vector3 direction = transform.TransformDirection(localDirection);
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hit;
        Debug.DrawRay(transform.position, direction);


        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.GetComponent<Ground>().type == GroundType.Arable)
                isOnArableGround = true;
            else
                isOnArableGround = false;
        }
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

    #endregion
}
