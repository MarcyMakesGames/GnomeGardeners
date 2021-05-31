using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
	[CreateAssetMenu(fileName = "HazardEC", menuName = "Events/Hazard Event Channel")]
	public class HazardEventChannelSO : ScriptableObject
	{
        public delegate void HazardAction(Sprite icon, float duration, float enterTime, float exitTime);
        public HazardAction OnEventRaised;

        public void RaiseEvent(Sprite icon, float duration, float enterTime, float exitTime)
        {
            OnEventRaised?.Invoke(icon, duration, enterTime, exitTime);
        }
    }
}
