using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
    public class Obstacle : MonoBehaviour, IInteractable
    {
        public bool debug = false;

        [Header("Designers")]
        [SerializeField] private bool canBeRemoved = true;
        [SerializeField] private ToolType removeTool = ToolType.Preparing;
        [SerializeField] private int numberOfHits = 3;
        [Header("Programmers")]
        [SerializeField] private Sprite spriteOnSoil;
        [SerializeField] private Sprite spriteOnRest;

        private GridCell cell;
        private SpriteRenderer spriteRenderer;
        private AudioSource audioSource;

        private int hitCounter;
        public GameObject AssociatedObject => gameObject;

        #region Unity Methods

        private void Start()
        {
            cell = GameManager.Instance.GridManager.GetClosestCell(transform.position);
            hitCounter = 1;
            spriteRenderer = GetComponent<SpriteRenderer>();
            audioSource = GetComponent<AudioSource>();


            if (cell.GroundType.Equals(GroundType.ArableSoil) || cell.GroundType.Equals(GroundType.FallowSoil))
            {
                spriteRenderer.sprite = spriteOnSoil;
            }
            else
            {
                spriteRenderer.sprite = spriteOnRest;
            }

            AssignOccupant();
        }

        #endregion

        #region Public Methods

        public void Interact(Tool tool = null)
        {
            DebugLogger.Log(this, "Being interacted with.");
            if (!canBeRemoved) { return; }
            if (tool == null) { return; }
            if (tool.Type == removeTool)
            {
                if (hitCounter < numberOfHits)
                {
                    ++hitCounter;
                }
                else
                {
                    gameObject.SetActive(false);
                    var gridPosition = GameManager.Instance.GridManager.GetClosestCell(transform.position).GridPosition;
                    GameManager.Instance.GridManager.ChangeTileOccupant(gridPosition, null);
                    DebugLogger.Log(this, "Obstacle removed.");
                    GameManager.Instance.AudioManager.PlaySound(SoundType.sfx_rock_breaking, audioSource);
                }
            }
        }

        public void AssignOccupant()
        {
            cell.AddCellOccupant(this);
        }

        #endregion
    }
}
