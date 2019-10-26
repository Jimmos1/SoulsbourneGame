using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnStateEnterBool : StateMachineBehaviour
{
    //Helper behaviour class for animations
    public string boolName;
    public bool toStatus;
    public bool resetOnExit;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(boolName, toStatus);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (resetOnExit)
            animator.SetBool(boolName, !toStatus);
    }
}
