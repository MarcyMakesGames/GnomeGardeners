using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
	public class InsectSearching : StateMachineBehaviour
	{
		public Insect insect;

		private readonly int hasTargetHash = Animator.StringToHash("HasTarget");

		//public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	    //{
		//}

		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			var hasTarget = insect.SetTargetToPlant();
			animator.SetBool(hasTargetHash, hasTarget);
	    }


	   // public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	   // {
	   //     
	   // }

	    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	    //{
	    //    // Implement code that processes and affects root motion
	    //}

	    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	    //{
	    //    // Implement code that sets up animation IK (inverse kinematics)
	    //}
	}
}