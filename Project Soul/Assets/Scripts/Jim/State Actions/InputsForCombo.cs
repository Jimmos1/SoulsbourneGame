using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputsForCombo : StateAction
{
    //Triggers & Bumpers
    bool Rb;
    bool Rt;
    bool Lb;
    bool Lt;

    bool LMDown;
    bool RMDown;

    //Logic
    bool isAttacking;

    PlayerStateManager states;
    //AttackInputs attackInput;

    public InputsForCombo(PlayerStateManager playerStates)
    {
        states = playerStates;
    }

    public override bool Execute()
    {
        states.horizontal = Input.GetAxis("Horizontal");
        states.vertical = Input.GetAxis("Vertical");

        if (states.canDoCombo == false)
            return false;

        Rb = Input.GetButton("RB");
        Rt = Input.GetButton("RT");
        Lb = Input.GetButton("LB");
        Lt = Input.GetButton("LT");

        LMDown = Input.GetMouseButtonDown(0);
        RMDown = Input.GetMouseButtonDown(1);

        if (Rb || Rt || Lb || Lt || LMDown || RMDown)
        {
            isAttacking = true;

            //if (Rb || LMDown)
            //{
            //    attackInput = AttackInputs.rb;
            //}
            //if (Rt || LMDown)
            //{
            //    attackInput = AttackInputs.rt;
            //}
            //if (Lb || RMDown)
            //{
            //    attackInput = AttackInputs.lb;
            //}
            //if (Lt || RMDown)
            //{
            //    attackInput = AttackInputs.lt;
            //}
        }

        if (isAttacking)
        {
            states.DoCombo();
            isAttacking = false;
        }

        return false;
    }
}
