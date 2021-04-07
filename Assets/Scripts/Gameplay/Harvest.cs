using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvest : MonoBehaviour, IHarvest
{
    public int pointValue;
    public int PointValue { get => pointValue; set => pointValue = value; }

    public string Name => throw new System.NotImplementedException();

    public bool Deliver(Vector3 origin, Vector3 direction, float distance)
    {
        // deliver harvest to truck; increase score by pointValue
        Ray ray = new Ray(origin, direction * distance);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            ITruck truck = hit.transform.GetComponent<ITruck>();

            if (truck != null)
            {
                truck.AddScore(pointValue);
                gameObject.SetActive(false);
                Debug.Log("Delivered harvest");
                return true;
            }
        }
        return false;
    }

    public void DropItem(Vector3 position, Vector3 direction)
    {
        gameObject.SetActive(true);
        transform.position = position;
        Debug.Log("Dropped harvest");
    }

    public void Interact(ITool tool = null)
    {
        if (tool == null)
            return;

        switch (tool.Type)
        {
            case ToolType.Carrying:
                gameObject.SetActive(false);
                Debug.Log("Took harvest");
                gameObject.SetActive(false);
                break;
            case ToolType.Digging:
                Debug.Log("Cannot dig harvest!");
                break;
            case ToolType.Watering:
                Debug.Log("Cannot water harvest!");
                break;
            case ToolType.Harvesting:
                Debug.Log("Cannot harvest harvest!");
                break;
            default:
                Debug.Log("Harvest.cs : tool not recognized!");
                break;
        }
    }
}
