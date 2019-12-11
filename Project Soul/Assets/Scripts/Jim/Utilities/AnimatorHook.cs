using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHook : MonoBehaviour
{
    //Helper Class for animations
    CharacterStateManager states;

    public virtual void Init(CharacterStateManager stateManager)
    {
        states = (CharacterStateManager)stateManager;
    }

    public void OnAnimatorMove()
    {
        OnAnimatorMoveOverride();
    }

    protected virtual void OnAnimatorMoveOverride()
    {
        if (states.useRootMotion == false)
            return;

        if (states.isGrounded && states.delta > 0)
        {
            Vector3 v = (states.anim.deltaPosition) / states.delta;
            v.y = states.rigidbody.velocity.y;
            states.rigidbody.velocity = v;
        }
    }

    public void OpenDamageCollider()
    {
        states.HandleDamageCollider(true);
    }

    public void CloseDamageCollider()
    {
        states.HandleDamageCollider(false);
    }

    public void EnableCombo()
    {
        states.canDoCombo = true;
    }

    public void EnableRotation()
    {
        states.canRotate = true;
    }

    public void DisableRotation()
    {
        states.canRotate = false;
    }
}
