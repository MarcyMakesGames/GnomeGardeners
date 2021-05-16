using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
	public abstract class Occupant : MonoBehaviour
	{
		protected GridCell cell;

        #region Unity Methods

        public void Start()
        {
			cell = GameManager.Instance.GridManager.GetClosestCell(transform.position);
			AssignOccupant(cell);
		}

        #endregion

        #region Public Methods

		public abstract void Interact(Tool tool);

		#endregion

		#region Protected Methods

		protected void RemoveOccupantFromCells()
        {
			RemoveOccupant(cell);
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
