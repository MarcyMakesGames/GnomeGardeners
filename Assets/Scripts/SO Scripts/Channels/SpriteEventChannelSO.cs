using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
	[CreateAssetMenu(fileName = "Sprite Event", menuName = "Events/Sprite Event Channel")]
	public class SpriteEventChannelSO : ScriptableObject
	{
		public delegate void SpriteAction(Sprite sprite);
		public SpriteAction OnEventRaised;

		public void RaiseEvent(Sprite sprite)
		{
			OnEventRaised?.Invoke(sprite);
		}
	}
}
