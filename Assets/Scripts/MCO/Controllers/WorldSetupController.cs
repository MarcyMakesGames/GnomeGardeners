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

    private void Configure()
    {
        GameManager.Instance.WorldSetupController = this;
        OnLevelStart.OnEventRaised += CreateWorld;
    }

    private void CreateWorld()
    {
        foreach (SetupObject obj in worldObjects)
        {
            var occupant = obj.occupant.GetComponent<IOccupant>();
            if ( occupant == null)
                break;

            Vector3 worldPosition = (Vector2)obj.position;
            Instantiate(obj.occupant, worldPosition, Quaternion.Euler(-20f, 0f, 0f));
            GameManager.Instance.GridManager.ChangeTileOccupant(obj.position, occupant);
        }
    }
}
