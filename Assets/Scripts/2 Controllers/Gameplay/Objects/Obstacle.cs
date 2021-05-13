using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour, IInteractable
{
    public bool debug = false;

    [Header("Designers")]
    [SerializeField] private bool canBeRemoved = true;
    [SerializeField] private ToolType removeTool = ToolType.Preparing;
    [SerializeField] private int numberOfHits = 3;
    [Header("Programmers")]
    [SerializeField] private Sprite spriteOnSoil;
    [SerializeField] private Sprite spriteOnRest;

    private GridCell cell;

    private int hitCounter;
    public GameObject AssociatedObject => gameObject;

    public void Interact(Tool tool = null)
    {
        Log("Being interacted with.");
        if (!canBeRemoved) { return; }
        if(tool == null) { return; }
        if(tool.Type == removeTool)
        {
            if(hitCounter < numberOfHits)
            {
                ++hitCounter;
            }
            else
            {
                gameObject.SetActive(false);
                var gridPosition = GameManager.Instance.GridManager.GetClosestCell(transform.position).GridPosition;
                GameManager.Instance.GridManager.ChangeTileOccupant(gridPosition, null);
                Log("Obstacle removed.");
            }
        }
    }

    public void AssignOccupant()
    {
        cell.AddCellOccupant(this);
    }

    private void Start()
    {
        cell = GameManager.Instance.GridManager.GetClosestCell(transform.position);
        hitCounter = 1;
        var spriteRenderer = GetComponent<SpriteRenderer>();

        if(cell.GroundType.Equals(GroundType.ArableSoil) || cell.GroundType.Equals(GroundType.FallowSoil))
        {
            spriteRenderer.sprite = spriteOnSoil;
        }
        else
        {
            spriteRenderer.sprite = spriteOnRest;
        }

        AssignOccupant();
    }

    private void Log(string msg)
    {
        Debug.Log("[Obstacle]: " + msg);
    }
}
