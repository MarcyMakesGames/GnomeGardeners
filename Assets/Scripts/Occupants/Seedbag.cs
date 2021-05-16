using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
    public class Seedbag : Occupant
    {
        public Seed[] dispensables = new Seed[3];

        #region Unity Methods

        private new void Start()
        {
            base.Start();
            dispensables[0] = new Seed(PoolKey.Plant_Sunflower);
            dispensables[1] = new Seed(PoolKey.Plant_Peppermint);
            dispensables[2] = new Seed(PoolKey.Plant_Strawberry);
        }

        #endregion

        #region Public Methods
        public override void Interact(Tool tool)
        {

        }

        public Seed GetRandomDispensable()
        {
            GameManager.Instance.AudioManager.PlaySound(SoundType.sfx_seedbag_dispense, GetComponent<AudioSource>());
            var randomIndex = Random.Range(0, dispensables.Length);
            var randomSeed = dispensables[randomIndex];
            DebugLogger.Log(this, "Dispensing." + randomSeed.ToString());
            return randomSeed;
        }

        #endregion
    }
}
