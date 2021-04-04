using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Truck : MonoBehaviour , IInteractable
{

    public int totalScore;

    // Start is called before the first frame update
    void Start()
    {
        totalScore = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact(ITool tool = null)
    {

    }
}
