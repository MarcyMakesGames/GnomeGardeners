using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
    public class Seedbag : Occupant
    {
        private bool debug = false;

        public GameObject[] dispensables;

        #region Unity Methods

        #endregion

        #region Public Methods
        public override void Interact(Tool tool)
        {

        }

        public IHoldable GetRandomDispensable()
        {
            GameManager.Instance.AudioManager.PlaySound(SoundType.sfx_seedbag_dispense, GetComponent<AudioSource>());
            var randomIndex = Random.Range(0, dispensables.Length);
            var randomDispensable = dispensables[randomIndex];
            DebugLogger.Log(this, "Dispensing." + randomDispensable.ToString());
            return randomDispensable.GetComponent<IHoldable>();
        }

        #endregion

        #region Private Methods



        private void DispenseItem(Vector2Int dropLocation)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
