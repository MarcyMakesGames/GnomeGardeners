using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour, IInteractable
{
    [SerializeField] private bool canBeRemoved;

    [SerializeField] private ToolType removeTool;
    private bool isRemoved;
    public GameObject AssociatedObject => gameObject;

    public void Interact(Tool tool = null)
    {
        if (!canBeRemoved) { return; }
        if(tool.Type == removeTool)
        {
            gameObject.SetActive(false);
            var gridPosition = GameManager.Instance.GridManager.GetClosestCell(transform.position).GridPosition;
            GameManager.Instance.GridManager.ChangeTileOccupant(gridPosition, null);
            isRemoved = true;
        }
    }

    public void AssignOccupant()
    {
        GameManager.Instance.GridManager.ChangeTileOccupant(GameManager.Instance.GridManager.GetClosestGrid(AssociatedObject.transform.position), this);
    }

    private void Start()
    {
        AssignOccupant();
    }
}
