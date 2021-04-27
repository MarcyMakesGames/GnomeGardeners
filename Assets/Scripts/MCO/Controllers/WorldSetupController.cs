using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSetupController : MonoBehaviour
{
    public List<SetupObject> worldObjects;
    public VoidEventChannelSO OnLevelStart;

    private void Awake()
    {
        if (GameManager.Instance.WorldSetupController == null)
        {
            Configure();
        }
    }

    private void OnDisable()
    {
        Dispose();
    }

    private void Configure()
    {
        GameManager.Instance.WorldSetupController = this;
        OnLevelStart.OnEventRaised += CreateWorld;
    }

    private void Dispose()
    {
        OnLevelStart.OnEventRaised -= CreateWorld;
    }

    private void CreateWorld()
    {
        foreach (SetupObject obj in worldObjects)
        {
            CreateObject(obj);
        }
        PaintWorld();
    }

    private void PaintWorld()
    {
        GameManager.Instance.GridManager.ChangeTile(new Vector2Int(0, 0), GroundType.FallowSoil);
        GameManager.Instance.GridManager.ChangeTile(new Vector2Int(8, 9), GroundType.FallowSoil);
        GameManager.Instance.GridManager.ChangeTile(new Vector2Int(9, 9), GroundType.FallowSoil);
        GameManager.Instance.GridManager.ChangeTile(new Vector2Int(10, 9), GroundType.FallowSoil);
        GameManager.Instance.GridManager.ChangeTile(new Vector2Int(8, 10), GroundType.FallowSoil);
        GameManager.Instance.GridManager.ChangeTile(new Vector2Int(9, 10), GroundType.FallowSoil);
        GameManager.Instance.GridManager.ChangeTile(new Vector2Int(10, 10), GroundType.FallowSoil);
        for (int i = 1; i < 9; ++i)
        {
            GameManager.Instance.GridManager.ChangeTile(new Vector2Int(i, 0), GroundType.Path);
            GameManager.Instance.GridManager.ChangeTile(new Vector2Int(9, i), GroundType.Path);
        }
        GameManager.Instance.GridManager.ChangeTile(new Vector2Int(0, 9), GroundType.Path);
    }

    private void CreateObject(SetupObject obj)
    {
        var gameObject = obj.gameObject;
        if (gameObject == null)
        {
            LogWarning("A SetupObject does not have an associated GameObject.");
            return;
        }
        var occupant = gameObject.GetComponent<IOccupant>();
        if (occupant == null)
        {
            LogWarning("A SetupObject's GameObject does not have a MonoBehaviour inheriting from IOccupant.");
            return;
        }

        Vector3 worldPosition = (Vector2)obj.position;
        var clone = Instantiate(gameObject, worldPosition, gameObject.transform.rotation);
        var occupantClone = clone.GetComponent<IOccupant>();
        GameManager.Instance.GridManager.ChangeTileOccupant(obj.position, occupantClone);
    }

    private void LogWarning(string msg)
    {
        Debug.LogWarning("[WorldSetupController]: " + msg);
    }
}
