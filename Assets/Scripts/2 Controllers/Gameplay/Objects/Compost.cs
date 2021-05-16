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
            Log("Interacting.");
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
            Log("Dispensing.");
            tool.heldItem = Dispensable;
            Log(dispensable.name);
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

        #region Private Methods

        private void Log(string msg)
        {
            if (!debug) { return; }
            Debug.Log("[Compost]: " + msg);
        }
        private void LogWarning(string msg)
        {
            if (!debug) { return; }
            Debug.LogWarning("[Compost]: " + msg);
        }

        #endregion
    }
}
