using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
    public class Obstacle : Occupant
    {
        public bool debug = false;

        [Header("Designers")]
        [SerializeField] private bool canBeRemoved = true;
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
            base.Start();
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
        }

        #endregion

        #region Public Methods

        public override void Interact(Tool tool)
        {
            DebugLogger.Log(this, "Being interacted with.");
            if (!canBeRemoved) { return; }
            if (tool == null) { return; }
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

        #endregion
    }
}
