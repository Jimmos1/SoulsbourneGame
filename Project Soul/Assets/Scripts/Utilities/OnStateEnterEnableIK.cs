using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnStateEnterEnableIK : StateMachineBehaviour
{
    AnimatorHook animatorHook;
    public bool status;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(animatorHook == null)
        {
            animatorHook = animator.gameObject.GetComponent<AnimatorHook>();
        }

        animatorHook.useIK = status;
    }
}
