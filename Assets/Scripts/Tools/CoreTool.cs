using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreTool : MonoBehaviour, ITool
{
    public void DropItem(Vector3 position)
    {
        // todo: drop tool
        Instantiate(gameObject, position, transform.rotation);
    }

    public void Interact(ITool tool = null)
    {
        // todo: gets equipped
        Destroy(gameObject);
    }

    public void UseTool()
    {
        //
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
