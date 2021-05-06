using UnityEngine;

public interface IOccupant
{
    public GameObject AssociatedObject { get; }

    public void AssignOccupant();
}

public interface IInteractable : IOccupant
{
    /// <summary>
    /// Interact on this object must be done through some kind of tool or handle a null tool circumstance.
    /// </summary>
    void Interact(Tool tool = null);
}

public interface IObjectDispenser : IInteractable
{
    IHoldable Dispensable { get; }
    void DispenseItem(Tool tool);
    void DispenseItem(Vector2Int dropLocation);
}

public interface IScoringArea : IInteractable
{
    void AddScore(int score);
}

public interface IHoldable
{
    bool IsBeingCarried { get; set; }
    Sprite SpriteInHand { get; set; }
    ItemType Type { get; set; }
}

public interface ITool : IInteractable, IHoldable
{
    ToolType Type { get; }
    void UseTool(Vector3 origin, Vector3 direction, float distance);
}

public interface IInteractionController
{
    GameObject Interact(Vector2 origin, Vector2 destination, Tool tool = null);
}
public interface ICommand
{
    /// <summary>
    /// A command is a reified method call. It is used to abstract the functionalities of tools.
    /// </summary>
    public void Execute(GridCell cell, Tool tool, GnomeController gnome);
}