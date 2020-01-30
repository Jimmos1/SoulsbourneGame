using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboInfo : StateMachineBehaviour
{
    public Combo[] combos;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {  
        Controller controller = animator.GetComponentInParent<Controller>();
        controller.LoadCombos(combos);
    }
}
