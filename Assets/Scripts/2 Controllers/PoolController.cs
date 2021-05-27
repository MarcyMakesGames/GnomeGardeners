using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
    public class PoolController : MonoBehaviour
    {
        [SerializeField] private PoolObject[] poolObjects;

        private int length;

        public PoolObject[] PowerupTotal { get => poolObjects; }

        public GameObject GetObjectFromPool(Vector2 position, Quaternion rotation, PoolKey key)
        {
            GameObject objectToReturn = null;

            for (int i = 0; i < length; i++)
            {
                var obj = poolObjects[i];

                if (obj.Key.Equals(key))
                {
                    objectToReturn = obj.GetObject();
                    break;
                }
            }

            var objTransform = objectToReturn.transform;

            objTransform.position = position;
            objTransform.rotation = rotation;

            objectToReturn.SetActive(true);

            return objectToReturn;
        }

        public void SetPoolObjectsInactive()
        {
            foreach (Transform obj in GetComponentsInChildren<Transform>())
                obj.gameObject.SetActive(false);
        }

        private void Awake()
        {
            length = poolObjects.Length;
            for (int i = 0; i < length; i++)
            {
                poolObjects[i].Init(transform);
            }

            if (GameManager.Instance.PoolController == null)
                GameManager.Instance.PoolController = this;
        }
    }

}
