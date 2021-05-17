using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
    public class Seedbag : Occupant
    {
        public bool onlySunflowerSeed = true;

        public Seed sunflowerSeed;

        public Seed[] allSeeds = new Seed[3];

        #region Unity Methods

        private new void Start()
        {
            base.Start();
            allSeeds[0] = new Seed(PoolKey.Plant_Sunflower);
            allSeeds[1] = new Seed(PoolKey.Plant_Peppermint);
            allSeeds[2] = new Seed(PoolKey.Plant_Strawberry);

            sunflowerSeed = new Seed(PoolKey.Plant_Sunflower);
        }

        #endregion

        #region Public Methods
        public override void Interact(Tool tool)
        {

        }

        public Seed GetRandomDispensable()
        {
            GameManager.Instance.AudioManager.PlaySound(SoundType.sfx_seedbag_dispense, GetComponent<AudioSource>());
            var randomIndex = Random.Range(0, allSeeds.Length);
            var randomSeed = allSeeds[randomIndex];
            DebugLogger.Log(this, "Dispensing." + randomSeed.ToString());
            return randomSeed;
        }

        public Seed GetSunflowerSeed()
        {
            GameManager.Instance.AudioManager.PlaySound(SoundType.sfx_seedbag_dispense, GetComponent<AudioSource>());
            DebugLogger.Log(this, "Dispensing." + sunflowerSeed.ToString());
            return sunflowerSeed;
        }

        #endregion
    }
}
