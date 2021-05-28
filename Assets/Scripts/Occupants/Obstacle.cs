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
        [SerializeField] private GameObject onSoil;
        [SerializeField] private GameObject onRest;

        private GridCell cell;
        private AudioSource audioSource;
        private SpriteRenderer spriteRenderer;
        private Animator animator;
        private bool isDestroyed;

        private int hitCounter;
        public GameObject AssociatedObject => gameObject;

        #region Unity Methods

        private void Start()
        {
            base.Start();
            cell = GameManager.Instance.GridManager.GetClosestCell(transform.position);
            hitCounter = 1;
            audioSource = GetComponent<AudioSource>();

            if (cell.GroundType.Equals(GroundType.ArableSoil) || cell.GroundType.Equals(GroundType.FallowSoil))
            {
                onSoil.SetActive(true);
                animator = onSoil.GetComponent<Animator>();
            }
            else
            {
                onRest.SetActive(true);
                animator = onRest.GetComponent<Animator>();
            }

            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = null;
        }
        protected override void Update()
        {
            base.Update();
            if(isDestroyed)
                Destroy(gameObject);
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
                animator.Play("rock_hitting");
            }
            else
            {
                animator.Play("rock_breaking");
                var gridPosition = GameManager.Instance.GridManager.GetClosestCell(transform.position).GridPosition;
                GameManager.Instance.GridManager.ChangeTileOccupant(gridPosition, null);
                DebugLogger.Log(this, "Obstacle removed.");
                GameManager.Instance.AudioManager.PlaySound(SoundType.sfx_rock_breaking, audioSource);
            }
        }

        public override void FailedInteraction()
        {
            throw new System.NotImplementedException();
        }

        public void SetDestroyed()
        {
            isDestroyed = true;
        }

        #endregion
    }
}
