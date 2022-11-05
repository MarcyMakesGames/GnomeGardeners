using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
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
        [SerializeField] private float interactRange = 2f;

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
        private Vector2 interactDir;
        private InputActions inputs;
        private GridCell currentCell;
        private Rigidbody2D rigidBody;
        private bool receiveGameInput;
        private AudioSource audioSource;
        private Animator currentAnimator;
        private string currentPrefix;
        private GridCell interactionCell;
        private PlayerConfig playerConfig;
        private GroundType currentGroundType;
        private Vector2Int previousGridPosition;
        private Vector2 interactionPos;

        private Animator animatorBack;
        private Animator animatorRight;
        private Animator animatorFront;
        private Animator animatorLeft;

        private SpriteResolver[] resolvers = new SpriteResolver[4];
        private SpriteRenderer[] itemRenderers = new SpriteRenderer[4];
        private SpriteRenderer[] toolRenderers = new SpriteRenderer[4];
        private IntEventChannelSO OnPlayerEquippingToolEvent;
        private IntEventChannelSO OnToolUnequippedByPlayerEvent;


        #region Unity Methods

        private void Awake()
        {
            inputs = new InputActions();
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
            rigidBody = GetComponent<Rigidbody2D>();
            audioSource = GetComponent<AudioSource>();
            receiveGameInput = true;
            previousGridPosition = new Vector2Int();
            moveDir = Vector2.zero;
            interactDir = Vector2.zero;
            
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
            
            animatorBack = gnomeBack.GetComponent<Animator>();
            animatorRight = gnomeRight.GetComponent<Animator>();
            animatorFront = gnomeFront.GetComponent<Animator>();
            animatorLeft = gnomeLeft.GetComponent<Animator>();
            currentAnimator = animatorFront;

            OnPlayerEquippingToolEvent = Resources.Load<IntEventChannelSO>("Channels/PlayerEquippingToolEC");
            OnToolUnequippedByPlayerEvent = Resources.Load<IntEventChannelSO>("Channels/ToolUnequippedByPlayerEC");

        }
        private void FixedUpdate()
        {
            if (moveSpeed > minimumSpeed)
                moveSpeed -= moveSpeed * slowdownFactor;

            currentCell = GameManager.Instance.GridManager.GetClosestCell(transform.position);

            if (currentCell.GroundType.Equals(GroundType.Path))
                moveSpeed = pathSpeed;

            if (moveDir == Vector2.zero)
            {
                rigidBody.velocity = Vector3.zero + GameManager.Instance.HazardManager.MovementModifier;
            }
            rigidBody.velocity = ((Vector3)moveDir * moveSpeed) + GameManager.Instance.HazardManager.MovementModifier;
        }

        private void Update()
        {
            CheckInteractGround();
            CalculateInteractionCell();
            UpdateAnimation();
            PlayFootstepSound();
        }

        private void OnDestroy()
        {
            playerConfig.Input.onActionTriggered -= OnInputAction;
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
            var value = context.ReadValue<Vector2>();
            moveDir = value;
            if(value != Vector2.zero)
                interactDir = value;
        }

        private void CheckInteractGround()
        {
            var gnomePosition = (Vector2)transform.position;
            var gnomeCell = GameManager.Instance.GridManager.GetClosestCell(gnomePosition);

            // Trample ground ability
            //if (gnomeCell.GroundType == GroundType.ArableSoil)
            //    GameManager.Instance.GridManager.ChangeTile(gnomeCell.GridPosition, GroundType.FallowSoil);

            //if (gnomeCell.Occupant != null)
            //{
            //    if (gnomeCell.Occupant.gameObject.GetComponent<Plant>() != null)
            //    {
            //        var plant = (Plant)gnomeCell.Occupant;
            //        plant.RemoveFromCell();
            //    }
            //}
        }

        private void UpdateAnimation()
        {
            var gnomeLooksDown = moveDir.x == 0f && moveDir.y < 0f;
            var gnomeLooksLeft = moveDir.x < 0f && moveDir.y == 0f;
            var gnomeLooksUp = moveDir.x == 0f && moveDir.y > 0f;
            var gnomeLooksRight = moveDir.x > 0f && moveDir.y == 0f;
            
            if (gnomeLooksDown)
            {
                gnomeFront.SetActive(true);
                gnomeLeft.SetActive(false);
                gnomeBack.SetActive(false);
                gnomeRight.SetActive(false);
                if (animatorFront != currentAnimator)
                    currentAnimator = animatorFront;
                currentPrefix = "front";
            }
            else if (gnomeLooksLeft)
            {
                gnomeFront.SetActive(false);
                gnomeLeft.SetActive(true);
                gnomeBack.SetActive(false);
                gnomeRight.SetActive(false);
                if (animatorLeft != currentAnimator)
                    currentAnimator = animatorLeft;
                currentPrefix = "left";
            }
            else if (gnomeLooksUp)
            {
                gnomeFront.SetActive(false);
                gnomeLeft.SetActive(false);
                gnomeBack.SetActive(true);
                gnomeRight.SetActive(false);
                if (animatorBack!= currentAnimator)
                    currentAnimator = animatorBack;
                currentPrefix = "back";
            }
            else if(gnomeLooksRight)
            {
                gnomeFront.SetActive(false);
                gnomeLeft.SetActive(false);
                gnomeBack.SetActive(false);
                gnomeRight.SetActive(true);
                if (animatorRight != currentAnimator)
                    currentAnimator = animatorRight;
                currentPrefix = "right";
            }

            if (moveDir.magnitude != 0)
                currentAnimator.SetBool("IsWalking", true);
            else
                currentAnimator.SetBool("IsWalking", false);
        }
        
        private void CalculateInteractionCell()
        {
            interactionPos = (Vector2)transform.position + interactDir * interactRange;
            interactionCell = GameManager.Instance.GridManager.GetClosestCell(interactionPos);
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
                        GameManager.Instance.PoolController.GetObjectFromPool(transform.position, Quaternion.identity, PoolKey.Particle_Grass_Single);
                        break;
                    case GroundType.Path:
                        GameManager.Instance.AudioManager.PlaySound(SoundType.sfx_footsteps_gravel, audioSource);
                        GameManager.Instance.PoolController.GetObjectFromPool(transform.position, Quaternion.identity, PoolKey.Particle_Dust);
                        break;
                    case GroundType.FallowSoil:
                    case GroundType.ArableSoil:
                        GameManager.Instance.AudioManager.PlaySound(SoundType.sfx_footsteps_dirt, audioSource);
                        GameManager.Instance.PoolController.GetObjectFromPool(transform.position, Quaternion.identity, PoolKey.Particle_Dust);

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
            GameManager.Instance.SceneController.HandleInput();
        }

        private void UseTool(GridCell cell)
        {
            var occupant = cell.Occupant;

            if (tool != null) // note: tool equipped and interacting on cell
            {
                tool.UseTool(cell);
                tool.UpdateItemRenderers(itemRenderers);
                tool.UpdateToolRenderers(toolRenderers);
                tool.PlayCorrespondingAnimation(currentAnimator, currentPrefix);
            }
            else if (tool == null && occupant != null) // note: no Tool equipped and interacting on occupant
            {
                occupant.FailedInteraction();
            }
        }

        private void EquipUnequip(GridCell cell)
        {
            var occupant = cell.Occupant;

            if (tool != null && occupant == null) // Unequip
            {
                tool.Unequip(interactionCell);
                tool.PlayCorrespondingAnimation(currentAnimator, currentPrefix);
                tool = null;
                SetAllResolvers("tools", "nada");
                SetAllItemRenderers(null);
                SetAllToolRenderers(null);
                OnToolUnequippedByPlayerEvent.RaiseEvent(playerConfig.PlayerIndex);
            }
            else if (tool == null && occupant != null) // Equip
            {
                Tool toolOnGround;
                if (occupant.TryGetComponent(out toolOnGround))
                {
                    tool = toolOnGround;
                    tool.UpdateSpriteResolvers(resolvers);
                    tool.UpdateItemRenderers(itemRenderers);
                    tool.UpdateToolRenderers(toolRenderers);
                    toolOnGround.Equip();
                    tool.PlayCorrespondingAnimation(currentAnimator, currentPrefix);
                    OnPlayerEquippingToolEvent.RaiseEvent(playerConfig.PlayerIndex);
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
