using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
    public class OccupantParticleController : MonoBehaviour
    {
        [SerializeField] private float overrideParticleDuration;

        private ParticleSystem particles;
        private float particleStartTime;

        private void Start()
        {
            particles = GetComponent<ParticleSystem>();
        }

        private void OnEnable()
        {
            particleStartTime = GameManager.Instance.Time.ElapsedTime;
        }

        private void Update()
        {
            ClearParticleSystem();
        }

        private void ClearParticleSystem()
        {
            if (overrideParticleDuration == 0 && GameManager.Instance.Time.GetTimeSince(particleStartTime) > particles.main.duration)
            {
                gameObject.SetActive(false);
            }
            else if (overrideParticleDuration != 0 && GameManager.Instance.Time.GetTimeSince(particleStartTime) > overrideParticleDuration)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
