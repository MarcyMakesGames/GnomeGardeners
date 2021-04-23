using UnityEngine;

public interface ICommand
{
    /// <summary>
    /// A command is a reified method call. In this game it is used to abstract the functionalities of the tools.
    /// </summary>
    public void Execute(GridCell cell, Tool tool);
}
