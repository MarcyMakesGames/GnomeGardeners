using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour, IInteractable, IHeldItem
{
    [SerializeField] private List<float> stageTimes;
    [SerializeField] private List<GameObject> harvests;
    [SerializeField] private List<Sprite> stageSprites;
    
    
    public int currentGrowthStage = 0;
    private float moisture = 0f;
    private float currentGrowTime = 0f;
    private bool isOnArableGround = false;
    private SpriteRenderer plantRenderer;

    public void Interact(ITool tool = null)
    {
        // todo: do behaviour based on tool and plant stage.
        //I.e. watering can + any stage, add to moisture
        //reaping tool + any stage returns an appropriate harvest
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
            default:
                Debug.Log("Plant.cs : tool not recognized!");
                break;
        }
    }

    public void DropItem(Vector3 position, Vector3 direction)
    {
        // todo: drop plant on ground; if plantbed then it grows
        Debug.Log("Dropped plant");
        gameObject.SetActive(true);
        transform.position = position + direction;
        currentGrowTime = GameManager.Instance.Time.ElapsedTime;
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
}
