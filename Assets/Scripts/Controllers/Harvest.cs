using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvest : MonoBehaviour
{
    private int pointValue = 50;
    public int PointValue { get => pointValue; set => pointValue = value; }
    private int objectIndex;
    public int ObjectIndex { get => objectIndex; set => objectIndex = value; }

    #region Public Methods
    public bool Deliver(Vector3 origin, Vector3 direction, float distance)
    {
        // deliver harvest to truck; increase score by pointValue
        Ray ray = new Ray(origin, direction);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distance))
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

    public void DropItem(Vector2 position)
    {
        GameObject harvest = GameManager.Instance.ObjectManager.Pool("Harvest").GetPooledObject();
        if(harvest != null)
        {
            harvest.transform.position = position;
            harvest.SetActive(true);
            Debug.Log("Dropped harvest");
        }
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
    #endregion
}
