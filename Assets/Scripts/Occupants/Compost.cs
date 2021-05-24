using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
    public class Compost : Occupant
    {
        private readonly bool debug = false;

        public Fertilizer dispensable;

        public IntEventChannelSO OnScoreAddEvent;

        #region Unity Methods

        private new void Start()
        {
            base.Start();
            dispensable = new Fertilizer();
        }

        protected override void Update()
        {
            base.Update();
        }

        #endregion

        #region Public Methods

        public override void Interact(Tool tool)
        {
            throw new System.NotImplementedException();
        }

        public Fertilizer DispenseItem()
        {
            DebugLogger.Log(this, "Dispensing." + dispensable.Name);
            return dispensable;
        }

        public void DispenseItem(Vector2Int dropLocation)
        {
            throw new System.NotImplementedException();
        }

        public void AddScore(int score)
        {
            OnScoreAddEvent.RaiseEvent(score);
        }

        public override void FailedInteraction()
        {
            GetPopUp(PoolKey.PopUp_Need_Fertilizing_Tool);
        }

        #endregion
    }
}
