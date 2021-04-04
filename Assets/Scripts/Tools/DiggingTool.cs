using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiggingTool : MonoBehaviour, ITool
{
    public void Interact(ITool tool = null)
    {
        // todo: gets equipped
        Destroy(gameObject);
    }

    public void UseTool()
    {
        // todo: digs up dirt
    }

    public void DropItem(Vector3 position)
    {
        // todo: drop tool
        Instantiate(gameObject, position, transform.rotation);
    }
}
