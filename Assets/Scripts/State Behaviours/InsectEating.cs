using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
	public class InsectEating : StateMachineBehaviour
	{
		public Insect insect;
		
		private readonly int hasTargetHash = Animator.StringToHash("HasTarget");        
		private readonly int isEatingHash = Animator.StringToHash("IsEating");
		private readonly int isFleeingHash = Animator.StringToHash("IsFleeing");


	    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	    {
	    }

	    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	    {
		    // result 0: lost plant while eating
		    // result 1: successfully eaten plant
		    // result 2: default update
		    var result = insect.EatPlant();
		    switch (result)
		    {
			    case 0:
				    animator.SetBool(hasTargetHash, false);
				    animator.SetBool(isEatingHash, false);
				    break;
			    case 1:
				    animator.SetBool(isFleeingHash, true);
				    break;
		    }
	    }


	    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	    {
	        
	    }

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