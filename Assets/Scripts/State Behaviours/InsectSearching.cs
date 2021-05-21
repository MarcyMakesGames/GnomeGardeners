using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
	public class InsectSearching : StateMachineBehaviour
	{
		public GameObject insectObject;

		private Insect insect;

		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	    {
            insect = insectObject.GetComponent<Insect>();
		}

		override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	    {
			insect.SetTargetToPlant();
	    }


	    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
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