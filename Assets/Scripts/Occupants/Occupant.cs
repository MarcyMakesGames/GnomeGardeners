using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
	[RequireComponent(typeof(BoxCollider2D))]
	public abstract class Occupant : MonoBehaviour
	{
		[SerializeField] protected float popUpDuration = 1f;
		public bool multiCellObject = false;
		protected GridCell cell;
		protected List<GridCell> occupantCells;
		protected Vector3 popUpOffset;
		protected GameObject popUp;
		protected float currentPopUpTime;

        #region Unity Methods

        public void Start()
        {
            InitOccupancy();

            AddOccupantToCells();
        }

        protected virtual void Update()
        {
			PopUpRemovalCountdown();
        }

        protected void OnEnable()
        {
			occupantCells = new List<GridCell>();
			InitOccupancy();
        }

        protected void OnDisable()
        {
			RemoveOccupantFromCells();
        }
        #endregion

        #region Public Methods

        public abstract void Interact(Tool tool);

		public abstract void FailedInteraction();
		#endregion

		#region Protected Methods

		protected void RemoveOccupantFromCells()
        {
			if (occupantCells == null)
				return;

			List<GridCell> oldCells = new List<GridCell>();

			foreach (GridCell cell in occupantCells)
            {
				RemoveOccupant(cell);
				oldCells.Add(cell);
			}

			foreach (GridCell cell in oldCells)
				if (occupantCells.Contains(cell))
					occupantCells.Remove(cell);
		}

		protected void AddOccupantToCells(GridCell newCell = null)
		{
			if (newCell != null)
				occupantCells.Add(newCell);

			foreach (GridCell cell in occupantCells)
				AssignOccupant(cell);
		}

		protected void GetPopUp(PoolKey popUpType)
		{
			ClearPopUp();

			GameObject newPopUp = GameManager.Instance.PoolController.GetObjectFromPool(transform.position + popUpOffset, Quaternion.identity, popUpType);
			popUp = newPopUp;

			currentPopUpTime = GameManager.Instance.Time.ElapsedTime;
		}

		protected void ClearPopUp()
		{
			if (popUp != null)
			{
				popUp.GetComponent<PopUpController>().EndPopUp();
				popUp = null;
			}
		}

		protected void PopUpRemovalCountdown()
		{
			if (popUp == null)
				return;

			if (GameManager.Instance.Time.GetTimeSince(currentPopUpTime) >= popUpDuration)
				ClearPopUp();
		}

		protected void InitOccupancy()
		{
			if (GameManager.Instance.GridManager == null)
				return;

			if (multiCellObject)
			{
				BoxCollider2D coll = GetComponent<BoxCollider2D>();

				if (coll.bounds.extents.x >= .4f)
				{
					cell = GameManager.Instance.GridManager.GetClosestCell(new Vector3(transform.position.x + coll.bounds.extents.x, transform.position.y, 0));
					occupantCells.Add(cell);
					cell = GameManager.Instance.GridManager.GetClosestCell(new Vector3(transform.position.x - coll.bounds.extents.x, transform.position.y, 0));
					occupantCells.Add(cell);
				}

				if (coll.bounds.extents.y >= .4f)
				{
					cell = GameManager.Instance.GridManager.GetClosestCell(new Vector3(transform.position.x, transform.position.y + coll.bounds.extents.y, 0));
					occupantCells.Add(cell);
					cell = GameManager.Instance.GridManager.GetClosestCell(new Vector3(transform.position.x, transform.position.y - coll.bounds.extents.y, 0));
					occupantCells.Add(cell);
				}

			}
			else
			{
				cell = GameManager.Instance.GridManager.GetClosestCell(transform.position);
				occupantCells.Add(cell);
				DebugLogger.Log(this, cell.GridPosition.ToString());
			}
		}
		#endregion

		#region Private Methods

		private void AssignOccupant(GridCell cell)
		{
			cell.Occupant = this;
		}

		private void RemoveOccupant(GridCell cell)
        {
			cell.Occupant = null;
		}
		#endregion
	}
}
