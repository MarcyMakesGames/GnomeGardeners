using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoringArea : MonoBehaviour, IScoringArea
{
    private Sprite[] plants;
    private int plantCount;

    public IntEventChannelSO OnScoreAddEvent;

    public Sprite[] Plants { set => plants = value; }
    public GameObject AssociatedObject { get => gameObject; }



    #region Unity Methods
    void Start()
    {
        AssignOccupant();
    }

    #endregion

    #region Public Methods

    public void AddScore(int score)
    {
        OnScoreAddEvent.RaiseEvent(score);
    }

    public void Interact(Tool tool = null)
    {
        Debug.Log("Interacted with scoring area");
        var harvest = (Plant)tool.heldItem;
        var harvestStage = harvest.CurrentStage;

        if(tool.Type == ToolType.Harvesting && harvestStage != null)
        {
            var score = harvestStage.pointValue;
            AddScore(score);
            AddSprite(harvestStage.sprite);
            ++plantCount;
            Debug.Log("Delivered plant");
        }
    }

    public void AssignOccupant()
    {
        GameManager.Instance.GridManager.ChangeTileOccupant(GameManager.Instance.GridManager.GetClosestGrid(AssociatedObject.transform.position), this);
    }
    #endregion

    #region Private Methods
    private void AddSprite(Sprite sprite)
    {
        plants[plantCount] = sprite;
    }

    #endregion
}
