using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCommand : ICommand
{
    private bool debug = false;
    public void Execute(GridCell cell, Tool tool, GnomeController gnome)
    {
        Log("Executing");
        /* if in front of plant
         *  water plant
         * if in front of pest
         *  add fright to pest
         */
        var occupant = cell.Occupant;
        if(occupant == null)
        {
            return;
        }

        var plant = cell.Occupant.AssociatedObject.GetComponent<Plant>();
        if (plant != null)
        {
            plant.AddToNeedValue(NeedType.Water, tool.waterAmount);
        }

        //var pest = cell.Occupant.GameObject.GetComponent<Pest>();
        //if(pest != null)
        //{
        //    pest.AddFright();
        //}
    }

    private void Log(string msg)
    {
        if (!debug) { return; }
        Debug.Log("[WaterCommand]: " + msg);
    }

    private void LogWarning(string msg)
    {
        if (!debug) { return; }
        Debug.LogWarning("[WaterCommand]: " + msg);
    }
}
