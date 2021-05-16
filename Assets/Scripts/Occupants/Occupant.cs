using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
	public abstract class Occupant : MonoBehaviour
	{
		protected List<GridCell> cells;

        #region Unity Methods

        public void Start()
        {
			cells = new List<GridCell>();
			cells.Add(GameManager.Instance.GridManager.GetClosestCell(transform.position));
			cells.ForEach(AssignOccupant);
		}

        #endregion

        #region Public Methods

		public abstract void Interact(Tool tool);

		#endregion

		#region Protected Methods

		protected void RemoveOccupantFromCells()
        {
			cells.ForEach(RemoveOccupant);
        }

		#endregion

		#region Private Methods

		private void AssignOccupant(GridCell cell)
		{
			cell.AddCellOccupant(this);
		}

		private void RemoveOccupant(GridCell cell)
        {
			cell.RemoveCellOccupant();
        }

		#endregion
	}
}
