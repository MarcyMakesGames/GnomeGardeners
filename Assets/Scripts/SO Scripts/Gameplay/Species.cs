using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
    [CreateAssetMenu(fileName = "Species", menuName = "Plants/Species")]
    public class Species : ScriptableObject
    {
        [Header("Gameplay Variables")]
        public string speciesName;
        public List<Stage> stages;
        public Stage decayedStage;
        public bool isDamaging;
        public bool isSpawning;
        public bool isFragile;
        public float growMultiplier;
        [Header("Visuals on Gnome")]
        public Sprite deadSprite;
        public Sprite prematureSprite;
        public Sprite harvestSprite;


        public Stage NextStage(int current)
        {
            int next = current + 1;
            if (next < stages.Count)
                return stages[next];
            else
                return stages[stages.Count - 1];
        }
    }
}
