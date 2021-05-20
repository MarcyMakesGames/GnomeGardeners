using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GnomeGardeners
{
    public class HazardClockUI : MonoBehaviour
    {
        public GameObject mask;
        public Image currentHazardIcon;

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
            image.SetNativeSize();
            var spawnedGO = Instantiate(gameObjectToSpawn, posRight.position, gameObjectToSpawn.transform.rotation, mask.transform);
            StartCoroutine(MoveAcross(spawnedGO, delay, duration, icon));
        }

        public IEnumerator MoveAcross(GameObject objectToMove, float delay, float duration, Sprite icon)
        {
            float elapsedTime = 0;
            while (elapsedTime < delay)
            {
                objectToMove.transform.position = Vector3.Lerp(posRight.position, posCenter.position, (elapsedTime / delay));
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            objectToMove.transform.position = posCenter.position;
            currentHazardIcon.sprite = icon;
            elapsedTime = 0;
            while (elapsedTime < duration)
            {
                objectToMove.transform.position = Vector3.Lerp(posCenter.position, posLeft.position, (elapsedTime / duration));
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            Destroy(objectToMove);
        }

    }
}
