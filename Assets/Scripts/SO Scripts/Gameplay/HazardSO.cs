using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
    [CreateAssetMenu(fileName = "HazardName", menuName = "Hazard")]
    public class HazardSO : ScriptableObject
    {
        [SerializeField]
        private List<HazardElementSO> hazardElements;
        private float duration = 0f;
        [SerializeField] private Sprite icon;

        public float Duration { get => duration; }
        public Sprite Icon { get => icon; }

        public void SpawnHazard(Vector3 spawnLocation, Vector3 despawnLocation)
        {

            foreach (HazardElementSO element in hazardElements)
            {
                element.SpawnElement(spawnLocation, despawnLocation);

                if (duration == 0f || element.Duration > duration)
                    duration = element.Duration;
            }
        }
    }
}
