using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHook : MonoBehaviour
{
    //Helper Class for animations
    Controller controller;
    bool isAI;

    Animator animator;

    public Vector3 deltaPosition;
    public bool canRotate;
    public bool canDoCombo;
    public bool canMove;
    public bool openDamageCollider;
    public bool hasLookAtTarget;
    public Vector3 lookAtPosition;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponentInParent<Controller>();
        if (controller == null)
        {
            isAI = true;
        }
        else
        {
            isAI = false;
        }

        RagdollStatus(false);
    } 

    void RagdollStatus(bool status)
    {
        Rigidbody[] ragdollRigids = GetComponentsInChildren<Rigidbody>();
        Collider[] ragdollColliders = GetComponentsInChildren<Collider>();

        foreach (Rigidbody r in ragdollRigids)
        {
            r.isKinematic = !status;
            r.gameObject.layer = 10; // Ragdoll layer
        }

        foreach (Collider c in ragdollColliders)
        {
            c.isTrigger = !status;
        }

        animator.enabled = !status;
    }

    public void OnAnimatorMove()
    {
        OnAnimatorMoveOverride();
    }

    protected virtual void OnAnimatorMoveOverride()
    {
     
        float delta = Time.deltaTime;

        if (!isAI)
        {
            if (controller == null)
                return;

            if (controller.isInteracting == false)
                return;

            if (controller.isGrounded && delta > 0)
            {
                deltaPosition = (animator.deltaPosition) / delta;
            }
        }
        else
        {
            deltaPosition = (animator.deltaPosition) / delta;
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (hasLookAtTarget)
        {
            animator.SetLookAtWeight(1f, 0.9f, 0.95f, 1f, 1f);
            animator.SetLookAtPosition(lookAtPosition);
        }
    }

    public void OpenCanMove()
    {
        canMove = true;
    }

    public void OpenDamageCollider()
    {
        openDamageCollider = true;
    }

    public void CloseDamageCollider()
    {
        openDamageCollider = false;
    }

    public void EnableCombo()
    {
        canDoCombo = true;
    }

    public void EnableRotation()
    {
        canRotate = true;
    }

    public void DisableRotation()
    {
        canRotate = false;
    }

    public void EnableRagdoll()
    {
        RagdollStatus(true);
    }
}
