using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
	[CreateAssetMenu(fileName = "Float EC", menuName = "Events/Float Event Channel")]
	public class FloatEventChannelSO : ScriptableObject
	{
        public delegate void FloatAction(float value);
        public FloatAction OnEventRaised;

        public void RaiseEvent(float value)
        {
            OnEventRaised?.Invoke(value);
        }
    }
}
