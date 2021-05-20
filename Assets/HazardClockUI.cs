using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GnomeGardeners
{
    public class HazardClockUI : MonoBehaviour
    {
        public GameObject mask;

        public Transform posLeft;
        public Transform posCenter;
        public Transform posRight;

        private HazardEventChannelSO OnNextHazard;

        private void Awake()
        {
            OnNextHazard = Resources.Load<HazardEventChannelSO>("Channels/NextHazardEC");
            OnNextHazard.OnEventRaised += SpawnHazardIcon;
        }

        private void OnDestroy()
        {
            OnNextHazard.OnEventRaised -= SpawnHazardIcon;
        }

        private void SpawnHazardIcon(Sprite icon, float duration, float delay)
        {
            var gameObjectToSpawn = new GameObject("Icon");
            var image = gameObjectToSpawn.AddComponent<Image>();
            image.sprite = icon;
            var spawnedGO = Instantiate(gameObjectToSpawn, posRight.position, gameObjectToSpawn.transform.rotation, mask.transform);
            StartCoroutine(MoveAcross(spawnedGO, delay, duration));
        }

        public IEnumerator MoveAcross(GameObject objectToMove, float delay, float duration)
        {
            float elapsedTime = 0;
            while (elapsedTime < delay)
            {
                transform.position = Vector3.Lerp(posRight.position, posCenter.position, (elapsedTime / delay));
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            transform.position = posCenter.position;

            elapsedTime = 0;
            while (elapsedTime < delay)
            {
                transform.position = Vector3.Lerp(posCenter.position, posLeft.position, (elapsedTime / delay));
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            Destroy(objectToMove);
        }

    }
}
