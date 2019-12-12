using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnExitBool_AI : StateMachineBehaviour
{
    //Helper behaviour class for AI animations
    public string boolName;
    public bool OnExit;

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
            animator.SetBool(boolName, OnExit);
    }
}
