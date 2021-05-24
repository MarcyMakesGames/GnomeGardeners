using UnityEngine;
using TMPro;

namespace GnomeGardeners
{
    public class TimerUI : CoreUIElement<float>
    {
        [SerializeField] private TMP_Text timerText;

        private FloatEventChannelSO OnCurrentLevelTimeEvent;

        private void Awake()
        {
            OnCurrentLevelTimeEvent = Resources.Load<FloatEventChannelSO>("Channels/CurrentLevelTimeEC");
            OnCurrentLevelTimeEvent.OnEventRaised += UpdateUI;
        }

        private void OnDestroy()
        {
            OnCurrentLevelTimeEvent.OnEventRaised -= UpdateUI;
        }

        public override void UpdateUI(float primaryData)
        {
            UpdateTimeAsString(timerText, primaryData);
        }



        protected override bool ClearedIfEmpty(float newData)
        {
            if (newData != 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
