using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.Experimental.U2D.Animation;
using static UnityEngine.InputSystem.InputAction;

namespace GnomeGardeners
{
    public class Gnome : MonoBehaviour
    {
        [Header("Designers")]
        [SerializeField] private bool isTestGnome = false;
        [SerializeField] private float minimumSpeed = 5f;
        [SerializeField] private float pathSpeed = 7f;
        [SerializeField] private float slowdownFactor = 0.01f;
        [SerializeField] private float speedUpTime = .5f;
        [SerializeField] private float interactRange = 1f;
        [SerializeField] private float dropRange = 1f;
        [SerializeField] private int inputFramesCapacity = 10;

        [Header("Programmers")]
        [SerializeField] private GameObject gnomeBack;
        [SerializeField] private GameObject gnomeRight;
        [SerializeField] private GameObject gnomeFront;
        [SerializeField] private GameObject gnomeLeft;
        [SerializeField] private SpriteResolver gnomeBackResolver;
        [SerializeField] private SpriteResolver gnomeRightResolver;
        [SerializeField] private SpriteResolver gnomeFrontResolver;
        [SerializeField] private SpriteResolver gnomeLeftResolver;
        [SerializeField] private SpriteRenderer itemRendererBack;
        [SerializeField] private SpriteRenderer itemRendererRight;
        [SerializeField] private SpriteRenderer itemRendererFront;
        [SerializeField] private SpriteRenderer itemRendererLeft;
        [SerializeField] private SpriteRenderer toolRendererBack;
        [SerializeField] private SpriteRenderer toolRendererRight;
        [SerializeField] private SpriteRenderer toolRendererFront;
        [SerializeField] private SpriteRenderer toolRendererLeft;
        

        private Tool tool;
        private float moveSpeed;
        private Vector2 moveDir;
        private Vector2 lookDir;
        private GnomeInput inputs;
        private GridCell currentCell;
        private Rigidbody2D rigidBody;
        private bool receiveGameInput;
        private AudioSource audioSource;
        private Animator currentAnimator;
        private GridCell interactionCell;
        private PlayerConfig playerConfig;
        private float currentSpeedUpTimer;
        private GroundType currentGroundType;
        private Vector2Int previousGridPosition;

        private Queue<Vector2> inputFrames;
        private SpriteResolver[] resolvers = new SpriteResolver[4];
        private SpriteRenderer[] itemRenderers = new SpriteRenderer[4];
        private SpriteRenderer[] toolRenderers = new SpriteRenderer[4];

        #region Unity Methods

        private void Awake()
        {
            inputs = new GnomeInput();
            rigidBody = GetComponent<Rigidbody2D>();
            audioSource = GetComponent<AudioSource>();
            receiveGameInput = true;
            previousGridPosition = new Vector2Int();
            moveDir = Vector2.zero;
            inputFrames = new Queue<Vector2>(inputFramesCapacity);
            for (int i = 0; i < inputFramesCapacity; ++i)
                inputFrames.Enqueue(Vector2.zero);
            
            resolvers[0] = gnomeBackResolver;
            resolvers[1] = gnomeFrontResolver;
            resolvers[2] = gnomeLeftResolver;
            resolvers[3] = gnomeRightResolver;
            itemRenderers[0] = itemRendererBack;
            itemRenderers[1] = itemRendererFront;
            itemRenderers[2] = itemRendererLeft;
            itemRenderers[3] = itemRendererRight;
            toolRenderers[0] = toolRendererBack;
            toolRenderers[1] = toolRendererFront;
            toolRenderers[2] = toolRendererLeft;
            toolRenderers[3] = toolRendererRight;
            SetAllResolvers("tools", "nada");
        }

        private void Start()
        {
            if (isTestGnome)
            {
                var testPlayerInput = GetComponent<PlayerInput>();
                var testPlayerConfig = new PlayerConfig(testPlayerInput);
                InitializePlayer(testPlayerConfig);
                testPlayerInput.uiInputModule = FindObjectOfType<InputSystemUIInputModule>();
            }
            moveSpeed = minimumSpeed;
        }
        private void FixedUpdate()
        {
            if (moveSpeed > minimumSpeed)
                moveSpeed -= moveSpeed * slowdownFactor;

            currentCell = GameManager.Instance.GridManager.GetClosestCell(transform.position);

            if (currentCell.GroundType.Equals(GroundType.Path))
                moveSpeed = pathSpeed;

            Move();

            CalculateLookDir();
        }

        private void Update()
        {
            CheckInteractGround();
            HighlightInteractionCell();
            UpdateAnimation();
            PlayFootstepSound();
        }

        void OnDrawGizmosSelected()
        {
            if(inputFrames != null)
            {
                foreach(Vector2 vector in inputFrames)
                    Gizmos.DrawLine(transform.position, transform.position + (Vector3)vector);
            }
        }

        #endregion

        #region Public Methods

        public void OnInputAction(CallbackContext context)
        {
            CheckInputReception();

            if (context.action.name == inputs.Player.Escape.name)
                OnInputEscape(context);

            if (!receiveGameInput) { return; }

            if (context.action.name == inputs.Player.Movement.name)
                OnInputMove(context);
            if (context.action.name == inputs.Player.Interact.name)
                OnInputEquipUnequip(context);
            if (context.action.name == inputs.Player.ToolUse.name)
                OnInputUseTool(context);
        }
        public void InitializePlayer(PlayerConfig incomingPlayer)
        {
            //This is where we would initialize the gnome skin.
            //skin = playerConfig.skin;
            playerConfig = incomingPlayer;
            playerConfig.Input.onActionTriggered += OnInputAction;
        }

        #endregion

        #region Private Methods

        private void CheckInputReception()
        {
            if (GameManager.Instance.SceneController.ActiveInGameUI == InGameUIMode.HUD)
            {
                receiveGameInput = true;
            }
            else
            {
                receiveGameInput = false;
            }
        }

        private void OnInputMove(CallbackContext context)
        {
            moveDir = context.ReadValue<Vector2>();
        }

        private void CheckInteractGround()
        {
            var gnomePosition = (Vector2)transform.position;
            var gnomeCell = GameManager.Instance.GridManager.GetClosestCell(gnomePosition);

            if (gnomeCell.GroundType == GroundType.ArableSoil)
                GameManager.Instance.GridManager.ChangeTile(gnomeCell.GridPosition, GroundType.FallowSoil);

            if (gnomeCell.Occupant != null)
            {
                if (gnomeCell.Occupant.gameObject.GetComponent<Plant>() != null)
                {
                    var plant = (Plant)gnomeCell.Occupant;
                    plant.RemoveFromCell();
                }
            }
        }

        private void UpdateAnimation()
        {
            if (lookDir.y < 0)
            {
                gnomeFront.SetActive(true);
                gnomeLeft.SetActive(false);
                gnomeBack.SetActive(false);
                gnomeRight.SetActive(false);
                currentAnimator = gnomeFront.GetComponent<Animator>();
            }
            else if (lookDir.y == 0 && lookDir.x < 0)
            {
                gnomeFront.SetActive(false);
                gnomeLeft.SetActive(true);
                gnomeBack.SetActive(false);
                gnomeRight.SetActive(false);
                currentAnimator = gnomeLeft.GetComponent<Animator>();
            }
            else if (lookDir.y > 0)
            {
                gnomeFront.SetActive(false);
                gnomeLeft.SetActive(false);
                gnomeBack.SetActive(true);
                gnomeRight.SetActive(false);
                currentAnimator = gnomeBack.GetComponent<Animator>();
            }
            else
            {
                gnomeFront.SetActive(false);
                gnomeLeft.SetActive(false);
                gnomeBack.SetActive(false);
                gnomeRight.SetActive(true);
                currentAnimator = gnomeRight.GetComponent<Animator>();
            }

            if (moveDir.magnitude != 0)
                currentAnimator.SetBool("IsWalking", true);
            else
                currentAnimator.SetBool("IsWalking", false);
        }

        private void HighlightInteractionCell()
        {
            var interactionPosition = (Vector2)transform.position + lookDir * interactRange;
            interactionCell = GameManager.Instance.GridManager.GetClosestCell(interactionPosition);
            GameManager.Instance.GridManager.HighlightTile(interactionCell.GridPosition, previousGridPosition);
            previousGridPosition = interactionCell.GridPosition;
        }

        private void PlayFootstepSound()
        {
            if (moveDir.magnitude != 0f)
            {
                if (!currentCell.GroundType.Equals(currentGroundType))
                    audioSource.Stop();
                currentGroundType = currentCell.GroundType;

                if (audioSource.isPlaying)
                    return;

                switch (currentGroundType)
                {
                    case GroundType.Grass:
                        GameManager.Instance.AudioManager.PlaySound(SoundType.sfx_footsteps_grass, audioSource);
                        break;
                    case GroundType.Path:
                        GameManager.Instance.AudioManager.PlaySound(SoundType.sfx_footsteps_gravel, audioSource);
                        break;
                    case GroundType.FallowSoil:
                    case GroundType.ArableSoil:
                        GameManager.Instance.AudioManager.PlaySound(SoundType.sfx_footsteps_dirt, audioSource);
                        break;
                }
            }
            else
            {
                audioSource.Stop();
            }
        }

        private void OnInputEquipUnequip(CallbackContext context)
        {
            if (context.performed) EquipUnequip(interactionCell);
        }

        private void OnInputUseTool(CallbackContext context)
        {
            if (context.performed) UseTool(interactionCell);
        }

        private void OnInputEscape(CallbackContext context)
        {
            if (context.performed) Escape();
        }

        private void Escape()
        {
            var activeInGamePanel = GameManager.Instance.SceneController.ActiveInGameUI;
            if (activeInGamePanel != InGameUIMode.PauseMenu)
            {
                activeInGamePanel = InGameUIMode.PauseMenu;
            }
            else
            {
                activeInGamePanel = InGameUIMode.HUD;
            }
            GameManager.Instance.SceneController.ActiveInGameUI = activeInGamePanel;
        }



        private void Move()
        {
            if (moveDir == Vector2.zero)
            {
                currentSpeedUpTimer = 0f;
                rigidBody.velocity = Vector3.zero + GameManager.Instance.HazardManager.MovementModifier;
            }

            currentSpeedUpTimer += Time.deltaTime;

            if (currentSpeedUpTimer >= speedUpTime)
            {
                rigidBody.velocity = ((Vector3)moveDir * moveSpeed) + GameManager.Instance.HazardManager.MovementModifier;
            }
        }

        private void CalculateLookDir()
        {
            if (moveDir == Vector2.zero) return;

            inputFrames.Enqueue(moveDir);
            inputFrames.Dequeue();

            Vector2[] vectors = new Vector2[inputFramesCapacity];
            vectors = inputFrames.ToArray();
            lookDir = new Vector2(
            vectors.Average(x => x.x),
            vectors.Average(x => x.y));
        }

        private void UseTool(GridCell cell)
        {
            var occupant = cell.Occupant;

            if (tool != null) // note: tool equipped and interacting on cell
            {
                tool.UseTool(cell);
                tool.UpdateItemRenderers(itemRenderers);
                tool.UpdateToolRenderers(toolRenderers);
            }
            else if (tool == null && occupant != null) // note: no Tool equipped and interacting on occupant
            {
                occupant.FailedInteraction();
            }
        }

        private void EquipUnequip(GridCell cell)
        {
            var occupant = cell.Occupant;

            if (tool != null && occupant == null)
            {
                var dropPosition = transform.position + (Vector3)lookDir * dropRange;
                var dropCell = GameManager.Instance.GridManager.GetClosestCell(dropPosition);
                tool.Unequip(dropCell);
                tool = null;
                SetAllResolvers("tools", "nada");
                SetAllItemRenderers(null);
                SetAllToolRenderers(null);
            }
            else if (tool == null && occupant != null)
            {
                Tool toolOnGround;
                if (occupant.TryGetComponent(out toolOnGround))
                {
                    tool = toolOnGround;
                    tool.UpdateSpriteResolvers(resolvers);
                    tool.UpdateItemRenderers(itemRenderers);
                    tool.UpdateToolRenderers(toolRenderers);
                    toolOnGround.Equip();

                }
            }
        }

        private void SetAllResolvers(string category, string label)
        {
            gnomeBackResolver.SetCategoryAndLabel(category, label);
            gnomeFrontResolver.SetCategoryAndLabel(category, label);
            gnomeRightResolver.SetCategoryAndLabel(category, label);
            gnomeLeftResolver.SetCategoryAndLabel(category, label);
        }
        
        private void SetAllItemRenderers(Sprite sprite)
        {
            itemRendererBack.sprite = sprite;
            itemRendererFront.sprite = sprite;
            itemRendererLeft.sprite = sprite;
            itemRendererRight.sprite = sprite;
        }

        private void SetAllToolRenderers(Sprite sprite)
        {
            toolRendererBack.sprite = sprite;
            toolRendererFront.sprite = sprite;
            toolRendererLeft.sprite = sprite;
            toolRendererRight.sprite = sprite;
        }
        #endregion
    }
}
