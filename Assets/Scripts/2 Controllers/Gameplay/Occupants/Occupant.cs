using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
	public class Occupant : MonoBehaviour
	{
		protected List<GridCell> cells;

        #region Unity Methods

        public void Start()
        {
			Configure();
        }

        #endregion

        #region Public Methods

        public void Configure()
        {
			cells = new List<GridCell>();
			cells.Add(GameManager.Instance.GridManager.GetClosestCell(transform.position));
			cells.ForEach(AssignOccupant);
		}

		public void Interact(Tool tool)
		{
			throw new System.NotImplementedException();
		}

		#endregion

		#region Private Methods

		private void AssignOccupant(GridCell cell)
		{
			cell.AddCellOccupant(this);
		}

		#endregion
	}
}
