using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour, IInteractable, IHeldItem
{
    [SerializeField] private string plantName;
    [SerializeField] private List<float> stageTimes;
    [SerializeField] private List<Harvest> harvests;
    [SerializeField] private List<Sprite> stageSprites;
    [SerializeField] private bool is2D;

    [SerializeField] private int dropAmount = 3;
    [SerializeField] private float dropScatter = 2f;
    
    
    public int currentGrowthStage = 0;
    private float moisture = 0f;
    private float currentMoisture = 0f;
    private float waterTime;
    private float currentGrowTime = 0f;
    [SerializeField] private bool isOnArableGround = false;
    private SpriteRenderer plantRenderer;

    protected int objectIndex;

    public string Name => plantName;

    public int ObjectIndex { get => objectIndex; set => objectIndex = value; }

    public void Interact(ITool tool = null)
    {
        // todo: do behaviour based on tool and plant stage.
        //I.e. watering can + any stage, add to moisture
        //reaping tool + any stage returns an appropriate harvest
        if (tool == null)
            return;

        switch (tool.Type)
        {
            case ToolType.Carrying:
                gameObject.SetActive(false);
                Debug.Log("Took plant");
                break;
            case ToolType.Digging:
                Debug.Log("Cannot dig plant!");
                break;
            case ToolType.Watering:
                moisture += 10f;
                waterTime = GameManager.Instance.Time.ElapsedTime;
                break;
            case ToolType.Harvesting:
                if (currentGrowthStage == stageSprites.Count)
                    Instantiate(harvests[harvests.Count - 1]);
                else
                    Instantiate(harvests[0]);
                break;
            default:
                Debug.Log("Plant.cs : tool not recognized!");
                break;
        }
    }

    public void DropItem(Vector2 position)
    {
        gameObject.SetActive(true);

        if(is2D)
        {
            GridManager gridManager = FindObjectOfType<GridManager>();
            transform.position = gridManager.GetClosestCell(position).WorldPosition;
            gameObject.SetActive(true);
        }
        else
        {
            transform.position = position;
        }
        CheckArableGround(position);

        Debug.Log("Planted at " + transform.position);
    }

    protected void Awake()
    {
        plantRenderer = GetComponent<SpriteRenderer>();
    }

    protected void Update()
    {
        Grow();
        ConsumeResources();
    }

    protected void Grow()
    {
        if (!isOnArableGround || moisture <= 0f || currentGrowthStage == stageSprites.Count - 1)
            return;

        if (GameManager.Instance.Time.GetTimeSince(currentGrowTime) >= stageTimes[currentGrowthStage] && moisture > 0f)
        {
            currentGrowTime = GameManager.Instance.Time.ElapsedTime;
            currentGrowthStage++;

            plantRenderer.sprite = stageSprites[currentGrowthStage];

            // temp: before harvest tool is implemented
            if (currentGrowthStage == stageTimes.Count - 1)
            {
                DropHarvest();
            }
        }
    }

    private void DropHarvest()
    {
        foreach (Harvest harvest in harvests)
        {
            Vector3 spawnPos = new Vector3(transform.position.x + UnityEngine.Random.Range(-dropScatter, dropScatter)
                                        , transform.position.y
                                        , transform.position.z + UnityEngine.Random.Range(-dropScatter, dropScatter));
            Instantiate(harvest, spawnPos, transform.rotation);
        }
    }

    protected void ConsumeResources()
    {
        currentMoisture = Mathf.Clamp(moisture - GameManager.Instance.Time.GetTimeSince(waterTime), 0, 100f);

        if (currentMoisture == 0f)
            moisture = currentMoisture;
    }
    
    protected void CheckArableGround(Vector3 checkPosition)
    {
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
        currentGrowTime = GameManager.Instance.Time.ElapsedTime;
    }
}
