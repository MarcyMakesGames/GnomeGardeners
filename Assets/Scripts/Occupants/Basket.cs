using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
    public class Basket : Occupant
    {
        [SerializeField] private Sprite emptyBasket;
        [SerializeField] private Sprite halfFullBasket;
        [SerializeField] private Sprite fullBasket;
        
        private int plantCount;
        private AudioSource basketSource;
        private SpriteRenderer spriteRenderer;

        public IntEventChannelSO OnScoreAddEvent;

        public GameObject AssociatedObject { get => gameObject; }



        #region Unity Methods

        private new void Start()
        {
            base.Start();
            basketSource = GetComponent<AudioSource>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        protected override void Update()
        {
            base.Update();
            UpdateSprites();
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
            SetPopUpLifetime(0f, false);
        }
        #endregion

        #region Private Methods
        private void UpdateSprites()
        {
            switch (GameManager.Instance.LevelManager.ScoreAmountForBasket())
            {
                case 0:
                    spriteRenderer.sprite = emptyBasket;
                    break;
                case 1:
                    spriteRenderer.sprite = halfFullBasket;
                    break;
                case 2:
                    spriteRenderer.sprite = fullBasket;
                    break;
            }
        }

        [ContextMenu("Debug: Add Score")]
        private void AddScore(int score)
        {
            OnScoreAddEvent.RaiseEvent(score);
        }
        #endregion
    }
}
