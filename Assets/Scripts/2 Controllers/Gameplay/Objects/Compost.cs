using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
    public class Compost : MonoBehaviour, IObjectDispenser, IScoringArea
    {
        private readonly bool debug = false;

        public GameObject dispensable;
        public IHoldable Dispensable { get => dispensable.GetComponent<IHoldable>(); }

        public GameObject AssociatedObject => gameObject;

        #region Unity Methods

        private void Start()
        {
            AssignOccupant();
        }

        #endregion

        #region Public Methods

        public void Interact(Tool tool = null)
        {
            DebugLogger.Log(this, "Interacting.");
            if (tool != null && tool.Type == ToolType.Harvesting)
            {
                DispenseItem(tool);
            }
        }

        public void AssignOccupant()
        {
            GameManager.Instance.GridManager.ChangeTileOccupant(GameManager.Instance.GridManager.GetClosestGrid(AssociatedObject.transform.position), this);
        }

        public void DispenseItem(Tool tool)
        {
            DebugLogger.Log(this, "Dispensing.");
            tool.heldItem = Dispensable;
            DebugLogger.Log(this, dispensable.name);
        }

        public void DispenseItem(Vector2Int dropLocation)
        {
            throw new System.NotImplementedException();
        }

        public void AddScore(int score)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
