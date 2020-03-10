using System;
using UnityEngine;
using UnityEngine.AI;

public class A_FourthMeleeAction : GoapAction
{
    private bool killEnemy = false;
    private GameObject enemy; // what enemy we attack
    private string playerTag = "Player";
    private string animAction = "Attack 3";
    private bool actionFlag = false;
    private float recoveryTimer;

    public float costRaisePerUse = 10f;

    public A_FourthMeleeAction()
    {
        addPrecondition("hasWeapon", true); // don't bother attacking when no weapon in hands
        addPrecondition("damagedEnemy", true);
        addPrecondition("severeDamagedEnemy", true);
        addPrecondition("trickEnemy", true);
        addEffect("killEnemy", true); // destroy his dreams
    }


    public override void reset()
    {
        killEnemy = false;
        enemy = null;
        actionFlag = false;
    }

    public override bool isDone()
    {
        return killEnemy; // TODO: TRACK LATER ISDONE CONDITION
    }

    public override bool requiresInRange()
    {
        return true; // yes we need to be near an enemy to attack.
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        // find the nearest player
        GameObject[] players = GameObject.FindGameObjectsWithTag(playerTag); //can support multiplayer
        GameObject closest = null;
        float closestDist = 0;

        foreach (GameObject player in players)
        {
            if (closest == null)
            {
                // first one, so choose it for now
                closest = player;
                closestDist = (player.transform.position - agent.transform.position).magnitude;
            }
            else
            {
                // is this one closer than the last?
                float dist = (player.transform.position - agent.transform.position).magnitude;
                if (dist < closestDist)
                {
                    // we found a closer one, use it
                    closest = player;
                    closestDist = dist;
                }
            }
        }
        if (closest == null)
            return false;

        enemy = closest;
        target = enemy;  //target is defined in GoapAction

        return closest != null;
    }

    public override bool perform(GameObject agent)
    {
        //TODO: WILL OPTIMIZE ANIM/NAVAGENT REFS IN LATER VERSION.
        Animator anim = (Animator)agent.GetComponentInChildren(typeof(Animator));
        NavMeshAgent navAgent = (NavMeshAgent)agent.GetComponentInChildren(typeof(NavMeshAgent));
        //GameObject damageCollider = agent.GetComponent<GoapCore>().damageCollider;
        AnimatorHook animatorHook = agent.GetComponentInChildren<AnimatorHook>();

        //navAgent.enabled = false;

        //anim.SetFloat("movement", 0f, 0.1f, Time.deltaTime);
        //anim.SetFloat("sideways", 0f, 0.1f, Time.deltaTime);

                                               //Becomes true only on animator exit script... 
        if (anim.GetBool("actionSuccess_AI")) //...if action is complete and successful
        {
            /*
             * Here we are sure we finished the animation
             * so it's possible we can get actual damagedEnemy
             * status from player and evaluate attack success.
             */
            killEnemy = true; //... effect is true so we can move to next action
            cost += costRaisePerUse;
            navAgent.enabled = true;
            animatorHook.CloseDamageColliders();
            anim.SetBool("actionSuccess_AI", false);
            Debug.Log("Attack 4 has ended!");

            return true;
        }

        //Becomes true during the period of attack OR getting disabled by enemy.
        if (anim.GetBool("isInteracting") != true) //Did we start animating an action...
        {

            if (actionFlag)                                  //Check if action is happening...
            {
                navAgent.enabled = false;             //...lets stop the agent for a bit shall we?

                anim.SetFloat("movement", 0f, 0.1f, Time.deltaTime);
                anim.SetFloat("sideways", 0f, 0.1f, Time.deltaTime);

                recoveryTimer -= Time.deltaTime;
                if (recoveryTimer <= 0)
                {
                    Debug.Log("Action Flag finished.");
                    actionFlag = false;
                }
            }
            else                                              //...else do my action
            {
                Vector3 dir = target.transform.position - agent.transform.position;
                dir.y = 0;
                dir.Normalize();
                float dot = Vector3.Dot(transform.forward, dir);

                //Debug.Log(animAction + " " + dot);

                if (dot < 0) //Checking if target is in behind so we turn...
                {
                    Transform mTransform = agent.transform;

                    navAgent.enabled = true;
                    navAgent.SetDestination(target.transform.position);

                    Vector3 relativeDirection = mTransform.InverseTransformDirection(navAgent.desiredVelocity);
                    relativeDirection.Normalize();

                    anim.SetFloat("movement", relativeDirection.z, 0.1f, Time.deltaTime);
                    anim.SetFloat("sideways", relativeDirection.x, 0.1f, Time.deltaTime);


                    mTransform.rotation = navAgent.transform.rotation;
                    return true;
                }
                else      //...otherwise perform action
                {
                    agent.GetComponent<GoapCore>().PlayTargetAnimation(this.animAction, true);
                    actionFlag = true;
                    animatorHook.OpenDamageColliders();
                    recoveryTimer = 2f; //TODO: Current action recovery time.
                    //PLAY SOUND/UI STUFF HERE
                }
            }
        }

        return true;
    }
}