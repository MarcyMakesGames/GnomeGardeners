using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
    public class Plant : Occupant, IHoldable
    {
        public SpriteRenderer spriteRenderer;
        public VoidEventChannelSO OnTileChanged;

        [SerializeField] private Species species;
        [SerializeField] private float timeToGrowVariation = 1f;

        private ItemType type;
        private bool isDecayed;
        private Stage currentStage;
        private bool isCurrentNeedFulfilled;
        private Sprite spriteInHand;
        private bool isBeingCarried;
        private float currentGrowTime;
        private bool isOnArableGround;
        private GridCell occupyingCell;
        private AudioSource audioSource;
        private float lastStageTimeStamp;
        private float randomizedTimeToGrow;

        public bool IsBeingCarried { get => isBeingCarried; set => isBeingCarried = value; }
        public Stage CurrentStage { get => currentStage; }
        public GameObject AssociatedObject { get => gameObject; }
        public Sprite SpriteInHand { get => spriteInHand; set => spriteInHand = value; }
        public ItemType Type { get => type; set => type = value; }


        #region Unity Methods

        private void Awake()
        {
            OnTileChanged.OnEventRaised += CheckOccupyingCell;
            OnTileChanged.OnEventRaised += CheckArableGround;
            audioSource = GetComponent<AudioSource>();

            Configure();
        }



        private new void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            TryGrowing();
            CheckNeedPopUp();
        }

        private void OnDestroy()
        {
            ClearPopUp();
            OnTileChanged.OnEventRaised -= CheckOccupyingCell;
            OnTileChanged.OnEventRaised -= CheckArableGround;
        }

        #endregion

        #region Public Methods

        public override void Interact(Tool tool)
        {

        }

        public Harvest HarvestPlant()
        {
            if (currentStage.isHarvestable)
            {
                var points = currentStage.points;
                RemoveOccupantFromCells();
                isBeingCarried = true;
                GameManager.Instance.AudioManager.PlaySound(SoundType.sfx_tool_cutting_plant);
                ClearPopUp();
                ReturnToPool();

                return new Harvest(points, spriteInHand);
            }
            return null;
        }

        public  void FulfillCurrentNeed(NeedType type)
        {
            if (type != currentStage.need.type)
            {
                DebugLogger.Log(this, "Incorrect need type.");
                return;
            }

            isCurrentNeedFulfilled = true;

            if (type == NeedType.Fertilizer)
                GameManager.Instance.AudioManager.PlaySound(SoundType.sfx_plant_fertilized, audioSource);
        }

        public void RemoveFromCell()
        {
            RemoveOccupantFromCells();
            ReturnToPool();
            ClearPopUp();
            GameManager.Instance.AudioManager.PlaySound(SoundType.sfx_plants_snapping, audioSource);
        }

        public void PlantSeed(GridCell cell)
        {
            if (currentStage.specifier != PlantStage.Seed)
                return;

            spriteRenderer.sprite = currentStage.sprite;
            transform.position = cell.WorldPosition;

            DebugLogger.Log(this, cell.GridPosition.ToString());
            AddOccupantToCells(cell);
            occupyingCell = cell;
            isBeingCarried = false;
            GameManager.Instance.AudioManager.PlaySound(SoundType.sfx_plant_planting, audioSource);
            CheckArableGround();
        }

        #endregion

        #region Private Methods

        private void TryGrowing()
        {
            if (isDecayed) { return; }

            if (!isOnArableGround) { return; }

            DebugLogger.LogUpdate(this, "Tries Growing on arable ground.");
            currentGrowTime = GameManager.Instance.Time.GetTimeSince(lastStageTimeStamp) * species.growMultiplier;

            if (currentGrowTime >= currentStage.timeToFulfillNeed)
            {
                if (isCurrentNeedFulfilled)
                {
                    if (currentGrowTime >= currentStage.timeToFulfillNeed + randomizedTimeToGrow)
                        AdvanceStages();
                }
                else
                    AdvanceToDecayedStage();
            }
        }

        private void CheckNeedPopUp()
        {

            if (popUp == null && !isCurrentNeedFulfilled && currentStage.need != null)
            {
                GetPopUp(currentStage.need.popUpType);
                PopUpController popUpControls = popUp.GetComponent<PopUpController>();

                popUpControls.InitAnimIconTimer(currentStage.timeToGrow);
            }
            else if (popUp != null && isCurrentNeedFulfilled)
            {
                ClearPopUp();
            }
        }

        private void AdvanceToDecayedStage()
        {
            DebugLogger.Log(this, "Grew into decayed stage.");
            currentStage = species.decayedStage;
            lastStageTimeStamp = GameManager.Instance.Time.ElapsedTime;
            spriteRenderer.sprite = species.decayedStage.sprite;
            name = "Decayed" + species.name;
            isCurrentNeedFulfilled = false;
            isDecayed = true;
            GameManager.Instance.GridManager.ChangeTile(occupyingCell.GridPosition, GroundType.FallowSoil);
            spriteInHand = species.deadSprite;
            GameManager.Instance.AudioManager.PlaySound(SoundType.sfx_plant_wilting, audioSource);
            GetPopUp(PoolKey.PopUp_Recycle);
        }

        private void AdvanceStages()
        {
            DebugLogger.Log(this, "Grew into stage: " + currentStage.specifier.ToString());
            var currentStageIndex = species.stages.IndexOf(currentStage);
            currentStage = species.NextStage(currentStageIndex);
            lastStageTimeStamp = GameManager.Instance.Time.ElapsedTime;
            spriteRenderer.sprite = currentStage.sprite;
            name = currentStage.name + " " + species.name;
            isCurrentNeedFulfilled = false;
            if(currentStage.specifier == PlantStage.Ripening) 
            { 
                spriteInHand = species.harvestSprite;
            }
            if(type == ItemType.Seed) 
            {
                type = ItemType.Harvest;
            }

            randomizedTimeToGrow = currentStage.timeToGrow + UnityEngine.Random.Range(-timeToGrowVariation, timeToGrowVariation);

            if(popUp != null)
            {
                popUp.gameObject.SetActive(false);
                popUp = null;
            }        
        }

        private void CheckArableGround()
        {
            if(occupyingCell.GroundType == GroundType.ArableSoil)
            {
                DebugLogger.Log(this, "Found Arable Ground!");
                isOnArableGround = true;
                lastStageTimeStamp = GameManager.Instance.Time.ElapsedTime;
            }
            else
            {
                isOnArableGround = false;
            }
        }

        private void ReturnToPool()
        {
            gameObject.SetActive(false);
            Configure();
        }

        private void CheckOccupyingCell()
        {
            occupyingCell = GameManager.Instance.GridManager.GetClosestCell(transform.position);
        }

        private void Configure()
        {
            currentStage = species.stages[0];
            isOnArableGround = false;
            spriteRenderer.sprite = currentStage.sprite;
            name = currentStage.name + " " + species.name;
            isBeingCarried = true;
            spriteInHand = species.prematureSprite;
            type = ItemType.Seed;
            randomizedTimeToGrow = currentStage.timeToGrow + UnityEngine.Random.Range(-timeToGrowVariation, timeToGrowVariation);
            isCurrentNeedFulfilled = false;
        }

        public override void FailedInteraction()
        {
            return;
        }

        #endregion
    }
}
