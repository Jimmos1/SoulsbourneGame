using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputControl : StateAction
{
    //Input Manager but named to InputControl to avoid conflicts
    PlayerStateManager s;

    //Triggers & Bumpers
    bool Rb;
    bool Rt;
    bool Lb;
    bool Lt;

    //Inventory
    bool inventoryInput;

    //Prompts
    bool b_Input;
    bool y_Input;
    bool x_Input;

    //DPad
    bool leftArrow;
    bool rightArrow;
    bool upArrow;
    bool downArrow;

    //Logic
    bool isAttacking;

    public InputControl(PlayerStateManager states)
    {
        s = states;
    }

    public override bool Execute()
    {
        bool retVal = false;
        isAttacking = false;

        s.horizontal = Input.GetAxis("Horizontal");
        s.vertical = Input.GetAxis("Vertical");
        Rb = Input.GetButton("RB");
        Rt = Input.GetButton("RT");
        Lb = Input.GetButton("LB");
        Lt = Input.GetButton("LT");

        inventoryInput = Input.GetButton("Inventory");

        b_Input = Input.GetButton("B");
        y_Input = Input.GetButtonDown("Y");
        x_Input = Input.GetButton("X");

        leftArrow = Input.GetButton("Left");
        rightArrow = Input.GetButton("Right");
        upArrow = Input.GetButton("Up");
        downArrow = Input.GetButton("Down");

        s.mouseX = Input.GetAxis("Mouse X");
        s.mouseY = Input.GetAxis("Mouse Y");

        s.moveAmount = Mathf.Clamp01(Mathf.Abs(s.horizontal) + Mathf.Abs(s.vertical));

        retVal = HandleAttacking();

        //temp
        if (Input.GetKeyDown(KeyCode.F) || y_Input)
        {
            if (s.lockOn)
            {
                s.OnClearLookOverride();
            }
            else
            {
                s.OnAssignLookOverride(s.target);
            }
        }

        return retVal;
    }

    bool HandleAttacking()
    {
        if (Rb || Rt || Lb || Lt || Input.GetMouseButtonDown(0))
        {
            isAttacking = true;
        }

        if (y_Input)
        {
            isAttacking = false;
        }

        if (isAttacking)
        {
            //Find the actual attack animation from the items etc.
            //play animation
            s.PlayTargetAnimation("Attack 1", true);
            s.ChangeState(s.attackStateId);
        }

        return isAttacking;
    }

}
