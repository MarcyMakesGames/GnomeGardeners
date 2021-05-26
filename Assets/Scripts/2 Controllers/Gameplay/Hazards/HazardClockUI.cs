using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GnomeGardeners
{
    public class HazardClockUI : MonoBehaviour
    {
        public Transform posLeft;
        public Transform posStart;
        public Transform posEnd;
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
            var spawnedGO = Instantiate(gameObjectToSpawn, posRight.position, gameObjectToSpawn.transform.rotation, transform);
            StartCoroutine(MoveAcross(spawnedGO, delay, duration));
        }

        private IEnumerator MoveAcross(GameObject objectToMove, float delay, float duration)
        {
            float elapsedTime = 0;
            while (elapsedTime < delay)
            {
                objectToMove.transform.position = Vector3.Lerp(posRight.position, posStart.position, (elapsedTime / delay));
                elapsedTime += GameManager.Instance.Time.DeltaTime;
                yield return new WaitForEndOfFrame();
            }
            objectToMove.transform.position = posStart.position;
            elapsedTime = 0;
            while (elapsedTime < duration)
            {
                objectToMove.transform.position = Vector3.Lerp(posStart.position, posEnd.position, (elapsedTime / duration));
                elapsedTime += GameManager.Instance.Time.DeltaTime;
                yield return new WaitForEndOfFrame();
            }
            objectToMove.transform.position = posEnd.position;
            elapsedTime = 0;
            while (elapsedTime < delay)
            {
                objectToMove.transform.position = Vector3.Lerp(posEnd.position, posLeft.position, (elapsedTime / delay));
                elapsedTime += GameManager.Instance.Time.DeltaTime;
                yield return new WaitForEndOfFrame();
            }
            Destroy(objectToMove);
        }

    }
}
