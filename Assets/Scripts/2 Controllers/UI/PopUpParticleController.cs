using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
    public class PopUpParticleController : MonoBehaviour
    {
        [SerializeField] private ParticleSystem popUpParticleSys;
        [SerializeField] private GameObject popUp;
        private bool endPopUp = false;
        private float duration;
        private float currentTime;

        public bool EndPopUp { get => endPopUp; set => EndPopUpObject(); }

        private void OnEnable()
        {
            duration = popUpParticleSys.main.duration;
            endPopUp = false;
        }

        private void Update()
        {
            CheckPopUpRemoval();
        }

        private void EndPopUpObject()
        {
            if (EndPopUp)
                return;

            popUpParticleSys.Play();
            currentTime = duration;
            duration = GameManager.Instance.Time.ElapsedTime;
            endPopUp = true;
        }

        private void CheckPopUpRemoval()
        {
            if (!EndPopUp)
                return;

            if (GameManager.Instance.Time.GetTimeSince(duration) > currentTime)
                popUp.SetActive(false);
        }
    }
}
