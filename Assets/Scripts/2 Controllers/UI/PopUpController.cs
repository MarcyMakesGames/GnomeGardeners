using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
    public class PopUpController : MonoBehaviour
    {
        [SerializeField] private Animator popUpAnimator;
        [SerializeField] private Animator iconAnimator;
        private float iconFlashTimer = 0f;
        private float currentFlashTimer = 0f;
        private bool iconFlash = false;

        public void SetPopUpTimer(float flashTimer, bool flash = true)
        {
            if (iconAnimator == null)
                return;

            if (flash == false)
                iconAnimator.enabled = false;
            else
                iconAnimator.enabled = true;
            
            
            iconFlash = flash;
            iconFlashTimer = GameManager.Instance.Time.ElapsedTime;
            currentFlashTimer = flashTimer;
            popUpAnimator.SetBool("EndPopUp", false);
        }

        public void EndPopUp()
        {
            popUpAnimator.SetBool("EndPopUp", true);
        }

        private void OnEnable()
        {
            if (iconAnimator != null)
                iconAnimator.speed = 1f;
        }

        private void Update()
        {
            UpdateAnimIconSpeed();
        }

        private void UpdateAnimIconSpeed()
        {
            if (iconAnimator == null || iconFlash == false)
                return;

            if(currentFlashTimer <= GameManager.Instance.Time.GetTimeSince(iconFlashTimer))
            {
                iconAnimator.speed = GameManager.Instance.Time.GetTimeSince(iconFlashTimer) / currentFlashTimer;
            }
        }
    }
}
