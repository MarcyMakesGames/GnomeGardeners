using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    protected Dictionary<string, ObjectPool> objectPoolDictionary;

    protected void Awake()
    {
        GameManager.Instance.ObjectManager = this;
        objectPoolDictionary = new Dictionary<string, ObjectPool>();
    }

    public void Add(string key, ObjectPool objectPool )
    {
        objectPoolDictionary.Add(key, objectPool);
    }

    public ObjectPool Pool(string key)
    {
        if (!objectPoolDictionary.ContainsKey(key))
        {
            Debug.LogError("ObjectPool with key does not exist.");
            return null;
        }

        return objectPoolDictionary[key];
    }
}
