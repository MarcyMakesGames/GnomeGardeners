using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
    public class WindSpawner : HazardSpawner
    {
        [SerializeField]
        private GameObject windPrefab;
        [SerializeField]
        private List<Transform> spawnAreas;
        [SerializeField]
        private float windStrength = 1f;

        private float startTime;
        private float spawnTime;

        private void Start()
        {
            InitWindSpawner();
        }

        private void Update()
        {
            CountdownTimer();
        }

        private void InitWindSpawner()
        {
            startTime = GameManager.Instance.Time.ElapsedTime;
            spawnTime = GameManager.Instance.Time.ElapsedTime;

            var movementModifier = new Vector3(Mathf.Clamp(despawnPosition.x - spawnPosition.x, -1f, 1f), Mathf.Clamp(despawnPosition.y - spawnPosition.y, -1f, 1f));

            GameManager.Instance.HazardManager.MovementModifier = movementModifier * windStrength;
        }

        private void CountdownTimer()
        {
            if (GameManager.Instance.Time.GetTimeSince(startTime) >= hazardDuration)
            {
                GameManager.Instance.HazardManager.MovementModifier = Vector3.zero;
                Destroy(gameObject);
            }

            if (GameManager.Instance.Time.GetTimeSince(spawnTime) >= timeBetweenSpawns)
            {
                SpawnWindObject();
                spawnTime = GameManager.Instance.Time.ElapsedTime;
            }
        }

        private void SpawnWindObject()
        {
            var randomSpawnPos = spawnAreas[Random.Range(0, spawnAreas.Count)];
            var positionDifferential = randomSpawnPos.position - transform.position;
            var targetDespawn = despawnPosition + positionDifferential;

            var windObject = Instantiate(windPrefab, randomSpawnPos.position, transform.rotation);
            var wind = windObject.GetComponent<Wind>();
            wind.despawnLocation = targetDespawn;
            wind.moveSpeed = spawnObjMoveSpeed;
        }
    }
}
