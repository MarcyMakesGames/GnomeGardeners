using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour, IOccupant
{
    [SerializeField] private ToolType type;
    [SerializeField] private int range = 1;

    public IHoldable heldItem;

    private ICommand use;
    private bool isEquipped;

    public GameObject GameObject { get => gameObject; }
    public ToolType Type { get => type; }

    void Start()
    {
        isEquipped = false;

        switch (type)
        {
            case ToolType.Preparing:
                use = new PrepareCommand();
                break;
            case ToolType.Seeding:
                use = new SeedCommand();
                break;
            case ToolType.Watering:
                use = new WaterCommand();
                break;
            case ToolType.Harvesting:
                use = new HarvestCommand();
                break;
        }
    }

    public void UseTool(GridCell cell)
    {
        use.Execute(cell, this);
    }

    public void Unequip(GridCell cell)
    {
        isEquipped = false;
        // take from interaction controller and put on ground
    }

    public void Equip(GridCell cell)
    {
        isEquipped = true;
        // take from ground and put in interaction controller
    }
}
