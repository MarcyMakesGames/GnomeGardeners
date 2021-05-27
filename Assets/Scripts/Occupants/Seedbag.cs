using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GnomeGardeners
{
    public class Seedbag : Occupant
    {
        [SerializeField] private PoolKey[] seedKeys;
        [SerializeField] private PoolKey[] popUpKeys;
        
        private List<Seed> seeds;
        private Queue<Seed> seedQueue;
        private int iterator;

        #region Unity Methods

        private new void Start()
        {
            base.Start();
            iterator = 0;
            seeds = new List<Seed>();
            seedQueue = new Queue<Seed>(2);
            for(int i = 0; i < seedKeys.Length; ++i)
            {
                seeds.Add(new Seed(seedKeys[i], popUpKeys[i]));
            }
            AddSeedToQueue();
            AddSeedToQueue();
        }

        protected override void Update()
        {
            base.Update();
            if (popUp == null)
            {
                var key = seedQueue.FirstOrDefault().PopUpKey;
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
            var seed = seedQueue.Dequeue();
            AddSeedToQueue();
            return seed;
        }

        public override void FailedInteraction()
        {
            GetPopUp(PoolKey.PopUp_Need_Seeding_Tool);
        }

        #endregion

        private void AddSeedToQueue()
        {
            seedQueue.Enqueue(seeds[iterator]);
            iterator++;
            if (iterator == seeds.Count)
                iterator = 0;
        }
    }
}
