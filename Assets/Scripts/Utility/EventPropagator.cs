using System.Collections;
using System.Collections.Generic;
using GnomeGardeners;
using UnityEngine;

	public class EventPropagator : MonoBehaviour
	{
		public void SetDestroyed()
		{
			transform.parent.GetComponent<Obstacle>().SetDestroyed();
		}
	}