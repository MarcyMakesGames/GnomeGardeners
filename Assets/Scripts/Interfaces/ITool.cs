using UnityEngine;

public interface ITool : IInteractable, IHeldItem
{
   void UseTool(Vector2 usePosition, Vector2 useDirection, float useRange);
}
