using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterStateManager : StateManager
{
    //Generic Class enemies can have this
    [Header("References")]
    public Animator anim;
    public new Rigidbody rigidbody;
    public AnimatorHook animHook;

    [Header("States")]
    public bool isGrounded;
    public bool useRootMotion;
    public bool lockOn;
    public Transform target;

    [Header("Controller Values")]
    public float vertical;
    public float horizontal;
    public float delta;
    public Vector3 rootMovement;

    public override void Init()
    {
        anim = GetComponentInChildren<Animator>();
        animHook = GetComponentInChildren<AnimatorHook>();
        rigidbody = GetComponentInChildren<Rigidbody>();
        anim.applyRootMotion = false;

        animHook.Init(this);
    }

    public void PlayTargetAnimation(string targetAnim, bool isInteracting)
    {
        anim.SetBool("isInteracting", isInteracting);
        anim.CrossFade(targetAnim, 0.2f);
    }

    public virtual void OnAssignLookOverride(Transform target)
    {
        this.target = target;
        if (target != null)
            lockOn = true;
    }

    public virtual void OnClearLookOverride()
    {
        lockOn = false;
    }

}
