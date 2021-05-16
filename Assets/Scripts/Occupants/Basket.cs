using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
    public class Basket : Occupant
    {
        private bool debug = false;

        private Sprite[] plants;
        private int plantCount;
        private AudioSource basketSource;

        public IntEventChannelSO OnScoreAddEvent;

        public Sprite[] Plants { set => plants = value; }
        public GameObject AssociatedObject { get => gameObject; }



        #region Unity Methods

        private new void Start()
        {
            base.Start();
            basketSource = GetComponent<AudioSource>();
        }

        #endregion

        #region Public Methods

        public override void Interact(Tool tool)
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

        #endregion

        #region Private Methods
        private void AddSprite(Sprite sprite)
        {
            plants[plantCount] = sprite;
        }

        [ContextMenu("Debug: Add Score")]
        private void AddScore(int score)
        {
            OnScoreAddEvent.RaiseEvent(score);
        }

        #endregion
    }
}
