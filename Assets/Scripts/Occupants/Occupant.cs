using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
	[RequireComponent(typeof(BoxCollider2D))]
	public abstract class Occupant : MonoBehaviour
	{
		public bool multiCellObject = false;
		protected GridCell cell;
		protected List<GridCell> cells = new List<GridCell>();

        #region Unity Methods

        public void Start()
        {
			if(multiCellObject)
			{
				BoxCollider2D coll = GetComponent<BoxCollider2D>();

				if(coll.bounds.extents.x >= .4f)
                {
					cell = GameManager.Instance.GridManager.GetClosestCell(new Vector3(transform.position.x + coll.bounds.extents.x, transform.position.y, 0));
					cells.Add(cell);
					cell = GameManager.Instance.GridManager.GetClosestCell(new Vector3(transform.position.x - coll.bounds.extents.x, transform.position.y, 0));
					cells.Add(cell);
				}

				if (coll.bounds.extents.y >= .4f)
				{
					cell = GameManager.Instance.GridManager.GetClosestCell(new Vector3(transform.position.x, transform.position.y + coll.bounds.extents.y, 0));
					cells.Add(cell);
					cell = GameManager.Instance.GridManager.GetClosestCell(new Vector3(transform.position.x, transform.position.y - coll.bounds.extents.y, 0));
					cells.Add(cell);
				}

				AddOccupantToCells();
			}
			else
            {
				cell = GameManager.Instance.GridManager.GetClosestCell(transform.position);
				AssignOccupant(cell);
			}
		}

        #endregion

        #region Public Methods

		public abstract void Interact(Tool tool);

		#endregion

		#region Protected Methods

		protected void RemoveOccupantFromCells()
        {
			foreach(GridCell cell in cells)
				RemoveOccupant(cell);
        }

		protected void AddOccupantToCells()
		{
			foreach (GridCell cell in cells)
            {
				AssignOccupant(cell);
				DebugLogger.Log(this, cell.GridPosition.ToString());
			}
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
