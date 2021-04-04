using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiggingTool : MonoBehaviour, ITool
{
    public void Interact(ITool tool = null)
    {
        // todo: gets equipped
        Debug.Log("Equipped digging tool.");
        gameObject.SetActive(false);
    }

    public void UseTool()
    {
        // todo: digs up dirt
    }

    public void DropItem(Vector3 position)
    {
        // todo: drop tool
        Debug.Log("Dropped digging tool.");
        gameObject.SetActive(true);
        transform.position = position;
    }
}
