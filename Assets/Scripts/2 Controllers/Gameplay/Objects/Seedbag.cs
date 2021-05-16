using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
    public class Seedbag : MonoBehaviour, IInteractable
    {
        private bool debug = false;

        public GameObject[] dispensables;
        public GameObject AssociatedObject { get => gameObject; }

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
            if (tool != null)
            {
                DispenseItem(tool);
            }
            else
            {
                return; // todo: implement drop on floor
            }

        }

        public void AssignOccupant()
        {
            GameManager.Instance.GridManager.ChangeTileOccupant(GameManager.Instance.GridManager.GetClosestGrid(AssociatedObject.transform.position), this);
        }

        #endregion

        #region Private Methods

        private void DispenseItem(Tool tool)
        {
            DebugLogger.Log(this, "Dispensing.");
            var randomIndex = Random.Range(0, dispensables.Length);
            var randomDispensable = dispensables[randomIndex];
            tool.heldItem = randomDispensable.GetComponent<IHoldable>();
            GameManager.Instance.AudioManager.PlaySound(SoundType.sfx_seedbag_dispense, GetComponent<AudioSource>());
            DebugLogger.Log(this, randomDispensable.ToString());
        }

        private void DispenseItem(Vector2Int dropLocation)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
