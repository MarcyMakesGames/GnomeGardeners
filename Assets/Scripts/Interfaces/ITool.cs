using UnityEngine;

public interface ITool : IInteractable, IHeldItem
{
    ToolType Type { get; }

    void UseTool(Ray ray, RaycastHit hit);
}

public enum ToolType
{
    Carrying,
    Digging,
    Watering,
    Harvesting,

    Count
}