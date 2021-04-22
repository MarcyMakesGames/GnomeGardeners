using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoringArea : MonoBehaviour, IInteractable
{
    public int TotalScore { get ; set; }

    private Sprite[] plants;
    private int plantCount;
    public Sprite[] Plants { set => plants = value; }

    public GameObject GameObject => gameObject;



    #region Unity Methods

    void Start()
    {
        TotalScore = 0;
    }

    #endregion

    #region Public Methods

    public void AddScore(int score)
    {
        TotalScore += score;

        FindObjectOfType<Scoreboard>().UpdateUI(TotalScore);
    }

    public void Interact(Tool tool = null)
    {
        var plant = (Plant)tool.heldItem;

        if(tool.Type == ToolType.Harvesting && plant != null)
        {
            var score = plant.CurrentStage.Score;
            AddScore(score);
            AddSprite(plant.CurrentStage.sprite);
            ++plantCount;
        }
    }

    #endregion

    #region Private Methods

    private void AddSprite(Sprite sprite)
    {
        plants[plantCount] = sprite;
    }

    #endregion
}
