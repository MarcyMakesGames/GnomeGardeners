using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;

namespace GnomeGardeners
{
    public abstract class Tool : Occupant
    {
        protected AudioSource audioSource;
        protected ToolType toolType;
        public AudioSource AudioSource { get => audioSource; }
        public ToolType ToolType { get => ToolType; }
        #region Unity Methods

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private new void Start()
        {
            base.Start();
        }

        #endregion

        #region Public Methods

        public abstract void UseTool(GridCell cell);

        public void Unequip(GridCell targetCell)
        {
            // todo: let the tool visually appear
            // temp:
            cell = targetCell;

            if(targetCell.Occupant == null)
            {
                gameObject.SetActive(true);
                transform.position = targetCell.WorldPosition;

                AddOccupantToCells(targetCell);

                PlayUnequipSound(targetCell.GroundType);
            }
        }


        public void Equip()
        {
            // todo: let the tool visually disappear
            // temp:
            RemoveOccupantFromCells();
            gameObject.SetActive(false);
        }

        public abstract void UpdateSpriteResolvers(SpriteResolver[] resolvers);

        #endregion

        #region Private Methods

        private void PlayUnequipSound(GroundType ground)
        {
            switch (ground)
            {
                case GroundType.Grass:
                    GameManager.Instance.AudioManager.PlaySound(SoundType.sfx_tool_thud_on_grass, audioSource);

                    break;
                case GroundType.Path:
                    GameManager.Instance.AudioManager.PlaySound(SoundType.sfx_tool_thud_on_gravel, audioSource);

                    break;
                case GroundType.FallowSoil:
                case GroundType.ArableSoil:
                    GameManager.Instance.AudioManager.PlaySound(SoundType.sfx_tool_thud_on_dirt, audioSource);

                    break;
            }
        }

        #endregion
    }
}
