using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
    public class Seedbag : Occupant
    {
        private bool debug = false;

        public GameObject[] dispensables;
        public GameObject AssociatedObject { get => gameObject; }

        #region Unity Methods

        private new void Start()
        {
            base.Start();
        }

        #endregion

        #region Public Methods
        public override void Interact(Tool tool)
        {
            DebugLogger.Log(this, "Interacting.");
            if (tool != null)
            {
                DispenseItem(tool);
            }
            else
            {
                return; // todo: implement drop on floor
            }

        }

        #endregion

        #region Private Methods

        private void DispenseItem(Tool tool)
        {
            DebugLogger.Log(this, "Dispensing.");
            var randomIndex = Random.Range(0, dispensables.Length);
            var randomDispensable = dispensables[randomIndex];
            tool.heldItem = randomDispensable.GetComponent<IHoldable>();
            GameManager.Instance.AudioManager.PlaySound(SoundType.sfx_seedbag_dispense, GetComponent<AudioSource>());
            DebugLogger.Log(this, randomDispensable.ToString());
        }

        private void DispenseItem(Vector2Int dropLocation)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
