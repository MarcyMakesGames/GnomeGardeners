using UnityEngine;

public interface ICommand
{
    /// <summary>
    /// A command is a reified method call. It is used to abstract the functionalities of tools.
    /// </summary>
    public void Execute(GridCell cell, Tool tool);
}
