using System;
using UnityEngine;
using UnityEngine.AI;

public class MeleeAttackAction : GoapAction
{
    private bool damagedEnemy = false;
    private GameObject enemy; // what enemy we attack
    private string playerTag = "Player";

    public MeleeAttackAction()
    {
        addPrecondition("hasWeapon", true); // don't bother attacking when no weapon in hands
        addEffect("damagedEnemy", true); // we damaged the enemy ---- MAYBE ATTEMPTDAMAGEDENEMY ----
    }


    public override void reset()
    {
        damagedEnemy = false;
        enemy = null;
    }

    public override bool isDone()
    {
        return damagedEnemy; // TODO: TRACK LATER ISDONE CONDITION
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
        Animator anim = (Animator)agent.GetComponent(typeof(Animator));
        //NavMeshAgent navAgent = (NavMeshAgent)agent.GetComponent(typeof(NavMeshAgent));
        anim.SetTrigger("MeleeAttack");
        //TODO: ATTACK EFFECTS (ANYTHING RELATED TO SOUND/UI/ETC)
        //navAgent.isStopped = true;
        damagedEnemy = true;
        return true;

        /*Animation will set isStopped to false after attack is finished
         */

        //Debug.Log(anim.GetCurrentAnimatorStateInfo(0).length + anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
        //yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length + anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
        //Debug.Log("Attack has ended!");
        //navAgent.isStopped = false;
        //TODO: SOME WAY TO GET CONFIRMATION PLAYER GOT DAMAGED?
    }
}
