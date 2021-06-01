using System;
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
        private VoidEventChannelSO OnLevelStartEvent;

        private List<GameObject> allIcons;

        private void Awake()
        {
            OnNextHazard = Resources.Load<HazardEventChannelSO>("Channels/NextHazardEC");
            OnLevelStartEvent = Resources.Load<VoidEventChannelSO>("Channels/LevelStartEC");
            OnNextHazard.OnEventRaised += SpawnHazardIcon;
            OnLevelStartEvent.OnEventRaised += DeleteAllIcons;
        }

        private void Start()
        {
            allIcons = new List<GameObject>();
        }

        private void OnDestroy()
        {
            OnNextHazard.OnEventRaised -= SpawnHazardIcon;
            OnLevelStartEvent.OnEventRaised -= DeleteAllIcons;
        }

        private void SpawnHazardIcon(Sprite icon, float duration, float enterTime, float exitTime)
        {
            var gameObjectToSpawn = new GameObject("Icon");
            var image = gameObjectToSpawn.AddComponent<Image>();
            image.sprite = icon;
            image.SetNativeSize();
            var spawnedGO = Instantiate(gameObjectToSpawn, posRight.position, gameObjectToSpawn.transform.rotation, transform.GetChild(1));
            allIcons.Add(spawnedGO);
            StartCoroutine(MoveAcross(spawnedGO, duration, enterTime, exitTime));
        }

        private IEnumerator MoveAcross(GameObject objectToMove, float duration, float enterTime, float exitTime)
        {
            yield return StartCoroutine(Move(objectToMove, posRight.position, posStart.position, enterTime));
            objectToMove.transform.position = posStart.position;
            yield return StartCoroutine(Move(objectToMove, posStart.position, posEnd.position, duration));
            objectToMove.transform.position = posEnd.position;
            yield return StartCoroutine(Move(objectToMove, posEnd.position, posLeft.position, exitTime));
            Destroy(objectToMove);
        }

        private IEnumerator Move(GameObject objectToMove, Vector3 from, Vector3 to, float time)
        {
            float elapsedTime = 0;
            while (elapsedTime < time)
            {
                if (objectToMove == null)
                    break;

                objectToMove.transform.position = Vector3.Lerp(from, to, (elapsedTime / time));
                elapsedTime += GameManager.Instance.Time.DeltaTime;
                yield return new WaitForEndOfFrame();
            }
        }

        private void DeleteAllIcons()
        {
            foreach (GameObject icon in allIcons)
            {
                Destroy(icon);
            }

            allIcons.Clear();
        }

    }
}
