using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
    public class GameTime : MonoBehaviour
    {
        private float currentTimer = 0f;
        private bool pauseTimer = false;
        public float ElapsedTime { get => currentTimer; }
        public bool PauseTimer { get => pauseTimer; set => pauseTimer = value; }

        #region Unity Methods

        private void Awake()
        {
            if (GameManager.Instance.Time == null)
            {
                GameManager.Instance.Time = this;
            }
        }

        private void Update()
        {
            CountUp();
        }

        #endregion

        #region Public Methods

        public float GetTimeSince(float time)
        {
            return ElapsedTime - time;
        }

        public void PauseTime()
        {
            pauseTimer = true;
        }

        public void ResumeTime()
        {
            pauseTimer = false;
        }

        #endregion

        #region Private Methods

        private void CountUp()
        {
            if (!pauseTimer)
                currentTimer += Time.deltaTime;
        }

        #endregion
    }
}
