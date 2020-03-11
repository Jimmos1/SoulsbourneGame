using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//v1.1
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
    public bool canBeParried;
    public Vector3 lookAtPosition;
    public bool useIK;

    public GameObject damageCollider;

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
            if (transform.tag == "Dragon")
            {
                //Vector3 newPosition = transform.position;
                //newPosition.z += animator.GetFloat("Runspeed") * Time.deltaTime;
                //newPosition = new Vector3(animator.deltaPosition.x, animator.deltaPosition.y, newPosition.z);
                //deltaPosition = (newPosition - transform.position) / delta;

                deltaPosition = (animator.deltaPosition) / delta;

            }
            else
            {
                deltaPosition = (animator.deltaPosition) / delta;
            }
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (hasLookAtTarget && !useIK)
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
        if (damageCollider != null)
        {
            damageCollider.SetActive(openDamageCollider);
        }
    }

    public void CloseDamageCollider()
    {
        openDamageCollider = false;
        if (damageCollider != null)
        {
            damageCollider.SetActive(openDamageCollider);
        }
    }
    //PATCH BECAUSE REASONS
    public void OpenDamageColliders()
    {
        openDamageCollider = true;
        if (damageCollider != null)
        {
            damageCollider.SetActive(openDamageCollider);
        }
    }

    public void CloseDamageColliders()
    {
        openDamageCollider = false;
        if (damageCollider != null)
        {
            damageCollider.SetActive(openDamageCollider);
        }
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

    public void EnableParryCollider()
    {
        if (!isAI)
        {
            controller.OpenParryCollider();
        }
    }

    public void EnableCanBeParried()
    {
        canBeParried = true;
    }

    public void DisableCanBeParried()
    {
        canBeParried = false;
    }

    public void EnableIK()
    {
        useIK = true;
    }

    public void DisableIK()
    {
        useIK = false;
    }

    public void ConsumeItem()
    {
        controller.ConsumeItem();
    }

    public void PlaySound(SoundManager.Sound sound)
    {
        if (!isAI) //this is done as a fix for overlapping sounds on footsteps
        {
            if (sound == SoundManager.Sound.PlayerMove)
            {
                if (!controller.isInteracting)
                {
                    SoundManager.PlaySound(sound, animator.transform.position);
                }
            }
            else
            {
                SoundManager.PlaySound(sound, animator.transform.position);
            }
        }
        else
        {
            SoundManager.PlaySound(sound, animator.transform.position);
        }
    }

    public void DrainAttribute()
    {
        if (!isAI)
        {
            controller.ComboAttributeDrain();
        }
    }
}
