using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour, IInteractable, IHeldItem
{
    [SerializeField] private List<float> stageTimes;
    [SerializeField] private List<GameObject> harvests;
    [SerializeField] private List<Sprite> stageSprites;
    [SerializeField] private bool is2D;
    
    
    public int currentGrowthStage = 0;
    private float moisture = 0f;
    private float currentGrowTime = 0f;
    [SerializeField] private bool isOnArableGround = false;
    private SpriteRenderer plantRenderer;

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

    public void DropItem(Vector3 position, Vector3 direction)
    {
        gameObject.SetActive(true);
        transform.position = position + direction;
        CheckArableGround(position + direction);
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
        if (!isOnArableGround || moisture <= 0f || currentGrowthStage == stageSprites.Count)
            return;

        if (GameManager.Instance.Time.GetTimeSince(currentGrowTime) >= stageTimes[currentGrowthStage] && moisture > 0f)
        {
            currentGrowTime = GameManager.Instance.Time.ElapsedTime;
            currentGrowthStage++;

            plantRenderer.sprite = stageSprites[currentGrowthStage];
        }
    }

    protected void ConsumeResources()
    {
        //Lower moisture or fertilizer over time?
    }
    
    protected void CheckArableGround(Vector3 checkPosition)
    {
        if(is2D)
        {
            GridManager gridManager = FindObjectOfType<GridManager>();

            GridCell cell = gridManager.GetGridCell(gridManager.GetClosestGrid(checkPosition));

            if (cell.GroundType == GroundType.Arable)
                isOnArableGround = true;
            else
                isOnArableGround = false;
        }

        else
        {
            //3D space check for arable ground.
        }

        currentGrowTime = GameManager.Instance.Time.ElapsedTime;

    }
}
