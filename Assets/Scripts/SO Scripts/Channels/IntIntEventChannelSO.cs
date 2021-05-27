using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
	[CreateAssetMenu(menuName = "Events/Int-Int Event Channel")]
	public class IntIntEventChannelSO : ScriptableObject
	{
		public delegate void IntIntAction(int value, int secondValue);
		public IntIntAction OnEventRaised;

		public void RaiseEvent(int value, int secondValue)
		{
			OnEventRaised?.Invoke(value, secondValue);
		}
	}
}
	