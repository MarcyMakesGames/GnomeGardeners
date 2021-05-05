using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour, IOccupant
{
    bool debug = true;

    public float waterAmount;
    [SerializeField] private ToolType type;

    public IHoldable heldItem;
    private ICommand use;

    private bool isEquipped;

    public GameObject AssociatedObject { get => gameObject; }
    public ToolType Type { get => type; }

    void Start()
    {
        isEquipped = false;

        switch (type)
        {
            case ToolType.Preparing:
                use = new PrepareCommand();
                Log("Preparing Tool initialized.");
                break;
            case ToolType.Seeding:
                use = new SeedCommand();
                Log("Seeding Tool initialized.");
                break;
            case ToolType.Watering:
                use = new WaterCommand();
                Log("Watering Tool initialized.");
                break;
            case ToolType.Harvesting:
                use = new HarvestCommand();
                Log("Harvesting Tool initialized.");
                break;
        }

        AssignOccupant();
    }

    public void UseTool(GridCell cell, GnomeController gnome)
    {
        use.Execute(cell, this, gnome);
    }

    public void Unequip(GridCell cell)
    {
        if (!isEquipped)
            return;

        // todo: let the tool visually appear
        // temp:
        gameObject.SetActive(true);

        isEquipped = false;
        transform.position = cell.WorldPosition;
        var occupant = gameObject.GetComponent<IOccupant>();
        cell.AddCellOccupant(occupant);

        Log("Unequipped " + type.ToString() + " Tool");
    }

    public void Equip(GridCell cell)
    {
        if (isEquipped)
            return;

        // todo: let the tool visually disappear
        // temp:
        gameObject.SetActive(false);

        isEquipped = true;
        cell.RemoveCellOccupant();

        Log("Equipped " + type.ToString() + " Tool");
    }

    public void AssignOccupant()
    {
        GameManager.Instance.GridManager.ChangeTileOccupant(GameManager.Instance.GridManager.GetClosestGrid(AssociatedObject.transform.position), this);
    }

    #region Private Methods

    private void Log(string msg)
    {
        if (debug)
            Debug.Log("[Tool]: " + msg);
    }

    private void LogWarning(string msg)
    {
        if (debug)
            Debug.LogWarning("[Tool]: " + msg);
    }

    #endregion
}
