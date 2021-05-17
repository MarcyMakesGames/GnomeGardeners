using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
    public abstract class Tool : Occupant
    {
        protected AudioSource audioSource;
        public AudioSource AudioSource { get => audioSource; }

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

        public abstract void UseTool(GridCell cell, Gnome gnome);

        public void Unequip(GridCell targetCell)
        {
            // todo: let the tool visually appear
            // temp:
            cell = targetCell;
            gameObject.SetActive(true);

            transform.position = targetCell.WorldPosition;
            targetCell.AddCellOccupant(this);

            PlayUnequipSound(targetCell.GroundType);
        }


        public void Equip()
        {
            // todo: let the tool visually disappear
            // temp:
            gameObject.SetActive(false);
            RemoveOccupantFromCells();
        }

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
