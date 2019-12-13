using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHook : MonoBehaviour
{
    //Helper Class for animations
    CharacterStateManager states;


    /*  
     *  this is for debugging so no animation errors pop up in the console THIS SHOULD BE ADRESSED AS SOON AS POSSIBLE
     *  
     *   if (transform.tag == "Enemy")
     *   return;
     *
     */

    public virtual void Init(CharacterStateManager stateManager)
    {
        if (transform.tag == "Enemy")
            return;
        states = (CharacterStateManager)stateManager;
    }

    public void OnAnimatorMove()
    {
        if (transform.tag == "Enemy")
            return;
        OnAnimatorMoveOverride();
    }

    protected virtual void OnAnimatorMoveOverride()
    {
        if (transform.tag == "Enemy")
            return;

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
        if (transform.tag == "Enemy")
            return;
        states.HandleDamageCollider(true);
    }

    public void CloseDamageCollider()
    {
        if (transform.tag == "Enemy")
            return;
        states.HandleDamageCollider(false);
    }

    public void EnableCombo()
    {
        if (transform.tag == "Enemy")
            return;
        states.canDoCombo = true;
    }

    public void EnableRotation()
    {
        if (transform.tag == "Enemy")
            return;
        states.canRotate = true;
    }

    public void DisableRotation()
    {
        if (transform.tag == "Enemy")
            return;
        states.canRotate = false;
    }

}
