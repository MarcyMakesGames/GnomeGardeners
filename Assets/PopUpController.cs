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

        public void InitAnimIconTimer(float flashTimer)
        {
            if (iconAnimator == null)
                return;

            iconFlashTimer = GameManager.Instance.Time.ElapsedTime;
            currentFlashTimer = flashTimer;
        }

        private void Update()
        {
            UpdateAnimIconSpeed();
        }

        private void UpdateAnimIconSpeed()
        {
            if(currentFlashTimer <= GameManager.Instance.Time.GetTimeSince(iconFlashTimer))
            {
                iconAnimator.speed = GameManager.Instance.Time.GetTimeSince(iconFlashTimer) / currentFlashTimer;
            }
        }

    }
}
