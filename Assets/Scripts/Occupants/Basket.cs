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
        }

        public void DeliverHarvest(int score) 
        {
            AddScore(score);
            ++plantCount;
            GameManager.Instance.AudioManager.PlaySound(SoundType.sfx_basket_receive, basketSource);
            DebugLogger.Log(this, "Delivered plant");

        }

        public override void FailedInteraction()
        {
            GetPopUp(PoolKey.PopUp_Need_Harvest_Tool);
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
