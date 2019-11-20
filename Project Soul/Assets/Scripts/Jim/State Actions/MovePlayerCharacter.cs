using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayerCharacter : StateAction
{
    //This is our PlayerController it also handles Animations and Rotations according to Camera might scrap Cinemachine and 
    //also add Navmesh for better control with enemies.
    PlayerStateManager states;

    public MovePlayerCharacter(PlayerStateManager playerStateManager)
    {
        states = playerStateManager;
    }

    public override bool Execute()
    {
        float frontY = 0;
        RaycastHit hit;
        Vector3 targetVelocity = Vector3.zero;

        if (states.lockOn)
        {
            targetVelocity = states.mTransform.forward * states.vertical * states.movementSpeed;
            targetVelocity += states.mTransform.right * states.horizontal * states.movementSpeed;
        }
        else
        {
            targetVelocity = states.mTransform.forward * states.moveAmount * states.movementSpeed;
        }

        Vector3 origin = states.mTransform.position + (targetVelocity.normalized * states.frontRayOffset);
        origin.y += .5f;
        Debug.DrawRay(origin, -Vector3.up, Color.red, .01f, false);
        if (Physics.Raycast(origin, -Vector3.up, out hit, 1.5f, states.ignoreForGroundCheck))
        {
            states.isGrounded = true;
            float y = hit.point.y;
            frontY = y - states.mTransform.position.y;
        }
        else
        {
            states.isGrounded = false;
        }

        Vector3 currentVelocity = states.rigidbody.velocity;

        if (states.isGrounded)
        {
            float moveAmount = states.moveAmount;

            if (moveAmount > 0.1f)
            {
                states.rigidbody.isKinematic = false;
                states.rigidbody.drag = 0;
                if (Mathf.Abs(frontY) > 0.02f)
                {
                    targetVelocity.y = ((frontY > 0) ? frontY + 0.2f : frontY - 0.2f) * states.movementSpeed;
                }
            }
            else
            {
                float abs = Mathf.Abs(frontY);

                if (abs > 0.02f)
                {
                    states.rigidbody.isKinematic = true;
                    targetVelocity.y = 0;
                    states.rigidbody.drag = 4;
                }

            }

            HandleRotation();
        }
        else
        {
            //states.collider.height = colStartHeight;
            states.rigidbody.isKinematic = false;
            states.rigidbody.drag = 0;
            targetVelocity.y = currentVelocity.y;

            HandleRotation(); //temp needs better "dropping" behaviour

        }

        HandleAnimations();

        Debug.DrawRay((states.mTransform.position + Vector3.up * .2f), targetVelocity, Color.green, 0.01f, false);
        states.rigidbody.velocity = targetVelocity;

        //	Debug.Log(targetVelocity);
        //states.rigidbody.velocity = Vector3.Lerp(currentVelocity, targetVelocity, states.delta * states.adaptSpeed);


        return false;
    }

    void HandleRotation()
    {
        Vector3 targetDir = Vector3.zero;
        float moveOverride = states.moveAmount;
        if (states.lockOn)
        {
            targetDir = states.target.position - states.mTransform.position;
            moveOverride = 1;
        }
        else
        {
            float h = states.horizontal;
            float v = states.vertical;

            targetDir = states.camera.transform.forward * v;
            targetDir += states.camera.transform.right * h;
        }

        targetDir.Normalize();
        targetDir.y = 0;
        if (targetDir == Vector3.zero)
            targetDir = states.mTransform.forward;

        Quaternion tr = Quaternion.LookRotation(targetDir);
        Quaternion targetRotation = Quaternion.Slerp(
            states.mTransform.rotation, tr,
            states.delta * moveOverride * states.rotationSpeed);

        states.mTransform.rotation = targetRotation;
    }

    void HandleAnimations()
    {
        if (states.isGrounded)
        {
            if (states.lockOn)
            {
                float v = Mathf.Abs(states.vertical);
                float f = 0;
                if (v > 0 && v <= .5f)
                    f = .5f;
                else if (v > 0.5f)
                    f = 1;

                if (states.vertical < 0)
                    f = -f;

                states.anim.SetFloat("forward", f, .2f, states.delta);

                float h = Mathf.Abs(states.horizontal);
                float s = 0;
                if (h > 0 && h <= .5f)
                    s = .5f;
                else if (h > 0.5f)
                    s = 1;

                if (states.horizontal < 0)
                    s = -1;

                states.anim.SetFloat("sideways", s, .2f, states.delta);
            }
            else
            {
                float m = states.moveAmount;
                float f = 0;
                if (m > 0 && m <= .5f)
                    f = .5f;
                else if (m > 0.5f)
                    f = 1;


                states.anim.SetFloat("forward", f, .2f, states.delta);
                states.anim.SetFloat("sideways", 0, .2f, states.delta);

            }


        }
        else
        {

        }
    }
}
