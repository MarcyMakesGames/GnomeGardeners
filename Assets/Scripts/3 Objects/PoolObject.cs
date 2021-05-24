using System;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
    [System.Serializable]
    public struct PoolObject
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private int amount;
        [SerializeField] private PoolKey key;

        private List<GameObject> gameObjects;
        private Transform parent;

        public PoolKey Key { get => key; }

        public void Init(Transform parent)
        {
            gameObjects = new List<GameObject>(amount);
            this.parent = parent;

            for (int i = 0; i < amount; i++)
                Generate();
        }

        public GameObject GetObject()
        {
            GameObject gameObject;

            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObject = gameObjects[i];

                if (!gameObject.activeSelf)
                    return gameObject;
            }

            return Generate();
        }

        private GameObject Generate()
        {
            GameObject obj = GameObject.Instantiate(prefab, parent);
            obj.SetActive(false);

            gameObjects.Add(obj);

            return obj;
        }
    }
}
