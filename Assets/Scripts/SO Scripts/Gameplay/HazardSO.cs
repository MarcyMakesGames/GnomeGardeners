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
        private float hazardDuration = 0f;

        public float HazardDuration { get => hazardDuration; }

        public void SpawnHazard(Vector3 spawnLocation, Vector3 despawnLocation)
        {
            foreach (HazardElementSO element in hazardElements)
            {
                element.SpawnElement(spawnLocation, despawnLocation);

                if (hazardDuration == 0f || element.Duration > hazardDuration)
                    hazardDuration = element.Duration;
            }
        }
    }
}
