using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
    public abstract class Tool : Occupant
    {
        private bool debug = false;

        private bool isEquipped;

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
            isEquipped = false;
        }

        #endregion

        #region Public Methods

        public abstract void UseTool(GridCell cell, GnomeController gnome);

        public void Unequip(GridCell cell)
        {
            if (!isEquipped)
                return;

            // todo: let the tool visually appear
            // temp:
            gameObject.SetActive(true);

            isEquipped = false;
            transform.position = cell.WorldPosition;
            var occupant = gameObject.GetComponent<Occupant>();
            cell.AddCellOccupant(occupant);

            PlayUnequipSound(cell.GroundType);
        }


        public void Equip()
        {
            // todo: let the tool visually disappear
            // temp:
            gameObject.SetActive(false);
            isEquipped = true;
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
