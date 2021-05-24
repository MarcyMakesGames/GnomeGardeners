using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;

namespace GnomeGardeners
{
	public class Pitchfork : Tool
	{
		private Fertilizer fertilizer;

		public override void Interact(Tool tool)
		{
			throw new System.NotImplementedException();
		}

		public override void FailedInteraction()
		{
			throw new System.NotImplementedException();
		}

		public override void UseTool(GridCell cell)
		{
			var occupant = cell.Occupant;
			if (occupant != null)
			{
				Plant plant;
				if (occupant.TryGetComponent(out plant) && fertilizer != null)
				{
					DebugLogger.Log(this, "Plant found while carrying Fertilizer!");
					plant.FulfillCurrentNeed(NeedType.Fertilizer);
					fertilizer = null;
					return;
				}
				
				Compost compost;
				if (occupant.TryGetComponent(out compost))
				{
					if (fertilizer == null)
					{
						DebugLogger.Log(this, "Taking fertilizer.");
						fertilizer = compost.DispenseItem();
						return;
					}
					else if (fertilizer != null)
					{
						DebugLogger.Log(this, "Discarding fertilizer.");
						fertilizer = null;
						return;
					}
				}
				
				occupant.FailedInteraction();
			}
		}

		public override void UpdateSpriteResolvers(SpriteResolver[] resolvers)
		{
			
			foreach (SpriteResolver resolver in resolvers)
			{
				resolver.SetCategoryAndLabel("tools", "compost");
			}
		}

		public override void UpdateToolRenderers(SpriteRenderer[] renderers)
		{
			foreach (SpriteRenderer renderer in renderers)
			{
				renderer.sprite = fertilizer?.Sprite;
			}
		}
	}
}
