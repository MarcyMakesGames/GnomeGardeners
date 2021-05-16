using UnityEngine;

namespace GnomeGardeners
{
    public interface IHoldable
    {
        bool IsBeingCarried { get; set; }
        Sprite SpriteInHand { get; set; }
        ItemType Type { get; set; }
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
}
