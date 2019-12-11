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

    bool LMDown;
    bool RMDown;

    //Inventory
    bool inventoryInput;

    //Prompts
    bool b_Input;
    bool y_Input;
    bool x_Input;
    bool FKey_Input;
    bool GKey_Input;

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

        b_Input = Input.GetButtonDown("B");
        y_Input = Input.GetButtonDown("Y");
        x_Input = Input.GetButtonDown("X");
        FKey_Input = Input.GetKeyDown(KeyCode.F); //need to add to input profile
        GKey_Input = Input.GetKeyDown(KeyCode.G); //need to add to input profile

        LMDown = Input.GetMouseButtonDown(0);
        RMDown = Input.GetMouseButtonDown(1);

        leftArrow = Input.GetButton("Left");
        rightArrow = Input.GetButton("Right");
        upArrow = Input.GetButton("Up");
        downArrow = Input.GetButton("Down");



        s.mouseX = Input.GetAxis("Mouse X");
        s.mouseY = Input.GetAxis("Mouse Y");

        s.moveAmount = Mathf.Clamp01(Mathf.Abs(s.horizontal) + Mathf.Abs(s.vertical));

        retVal = HandleAttacking();

        if (FKey_Input || x_Input)
        {
            if (s.lockOn)
            {
                s.OnClearLookOverride();
            }
            else
            {
                s.target = s.FindLockableTarget();
                if (s.target != null)
                    s.OnAssignLookOverride(s.target);
            }
        }

        //debug
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            s.movementSpeed += 5f;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            s.movementSpeed -= 5f;
        }

        if (Input.GetKeyDown(KeyCode.Space) || b_Input)
        {
            s.rigidbody.position += Vector3.up * 10;
        }
        //debug

        if (s.canDoCombo)
        {
            bool isInteracting = s.anim.GetBool("isInteracting");
            if (!isInteracting)
            {
                s.canDoCombo = false;
            }
        }


        return retVal;
    }

    bool HandleAttacking()
    {
        AttackInputs attackInput = AttackInputs.rt;

        if (Rb || Rt || Lb || Lt || LMDown || RMDown)
        {
            isAttacking = true;

            if (Rb || LMDown)
            {
                attackInput = AttackInputs.rb;
            }
            if (Rt || LMDown)
            {
                attackInput = AttackInputs.rt;
            }
            if (Lb || RMDown)
            {
                attackInput = AttackInputs.lb;
            }
            if (Lt || RMDown)
            {
                attackInput = AttackInputs.lt;
            }
        }

        if (y_Input || GKey_Input)
        {
            s.HandleTwoHanded();
        }

        if (isAttacking)
        {
            //Find the actual attack animation from the items etc.
            //play animation
            s.PlayTargetItemAction(attackInput);
            s.ChangeState(s.attackStateId);
        }

        return isAttacking;
    }

}
