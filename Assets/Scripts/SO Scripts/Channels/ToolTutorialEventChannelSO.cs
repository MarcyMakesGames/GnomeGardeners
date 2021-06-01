using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
	[CreateAssetMenu(fileName = "Tool Tutorial Event", menuName = "Events/Tool Tutorial Event Channel")]
	public class ToolTutorialEventChannelSO : ScriptableObject
	{
		public delegate void ToolAction(GameObject toolTutorial);
		public ToolAction OnEventRaised;

		public void RaiseEvent(GameObject toolTutorial)
		{
			OnEventRaised?.Invoke(toolTutorial);
		}
	}
}