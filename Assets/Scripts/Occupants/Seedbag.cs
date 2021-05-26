using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
    public class Seedbag : Occupant
    {
        [SerializeField] private bool dispensesRandomizedSeeds = false;
        [SerializeField] private PoolKey[] seedKeys;
        [SerializeField] private PoolKey[] popUpKeys;
        
        private List<Seed> seeds;
        private int currentSeedIndex;
        private int nextSeedIndex;
        private Seed nextSeed;

        #region Unity Methods

        private new void Start()
        {
            base.Start();
            seeds = new List<Seed>();
            foreach (PoolKey key in seedKeys)
                seeds.Add(new Seed(key));

            currentSeedIndex = 0;
            nextSeedIndex = 1;
        }

        protected override void Update()
        {
            base.Update();
            if (popUp == null)
            {
                var key = popUpKeys[nextSeedIndex];
                GetPopUp(key);
            }
        }
        
        #endregion

        #region Public Methods
        public override void Interact(Tool tool)
        {
            
        }

        public Seed GetSeed()
        {
            GameManager.Instance.AudioManager.PlaySound(SoundType.sfx_seedbag_dispense, GetComponent<AudioSource>());
            ClearPopUp();
            popUp = null;
            if (dispensesRandomizedSeeds)
            {
                var seed = nextSeed;
                nextSeedIndex = UnityEngine.Random.Range(0, seeds.Count);
                nextSeed = seeds[nextSeedIndex];
                return seed;
            }
            else
            {
                var seed = seeds[currentSeedIndex];
                currentSeedIndex++;
                if (currentSeedIndex >= seeds.Count)
                    currentSeedIndex = 0;
                nextSeedIndex++;
                if (nextSeedIndex >= seeds.Count)
                    nextSeedIndex = 0;
                return seed;
            }
        }

        public override void FailedInteraction()
        {
            GetPopUp(PoolKey.PopUp_Need_Seeding_Tool);
        }

        #endregion
    }
}
