using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
    public class ScoringArea : MonoBehaviour, IScoringArea
    {
        private bool debug = false;

        private Sprite[] plants;
        private int plantCount;
        private AudioSource basketSource;

        public IntEventChannelSO OnScoreAddEvent;

        public Sprite[] Plants { set => plants = value; }
        public GameObject AssociatedObject { get => gameObject; }



        #region Unity Methods
        void Start()
        {
            basketSource = GetComponent<AudioSource>();
            AssignOccupant();
        }

        #endregion

        #region Public Methods

        [ContextMenu("Debug: Add Score")]
        public void AddScore(int score)
        {
            OnScoreAddEvent.RaiseEvent(score);
        }

        public void Interact(Tool tool = null)
        {
            DebugLogger.Log(this, "Interacted with scoring area");
            var harvest = (Plant)tool.heldItem;
            if (harvest == null) { return; }

            var harvestStage = harvest.CurrentStage;
            if (tool.Type == ToolType.Harvesting && harvestStage != null)
            {
                var score = harvestStage.pointValue;
                AddScore(score);
                // AddSprite(harvestStage.sprite);
                ++plantCount;
                GameManager.Instance.AudioManager.PlaySound(SoundType.sfx_basket_receive, basketSource);
                DebugLogger.Log(this, "Delivered plant");
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
}
