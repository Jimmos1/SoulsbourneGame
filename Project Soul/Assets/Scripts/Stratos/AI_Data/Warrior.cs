using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;


/**
 * A generic Warrior class for Medieval setting.
 * You should subclass this for specific Warrior classes and implement
 * the createGoalState() method that will populate the goal for the GOAP
 * planner.
 * 
 * POTENTIAL SUBCLASSES INCLUDE: Knight, Berserker, Gladiator, etc.
 */
public abstract class Warrior : MonoBehaviour, IGoap
{
    public WarriorBackpackComponent backpack;
    public CombatStats combatStats;

    public NavMeshHit navHit;
    public NavMeshAgent navAgent;
    public RaycastHit rayHit;
    private Transform target;

    public float moveSpeed = 1;


    void Start()
    {
        if (backpack == null)
            backpack = this.gameObject.AddComponent<WarriorBackpackComponent>() as WarriorBackpackComponent;
        if (backpack.weapon == null)
        {
            GameObject prefab = Resources.Load<GameObject>(backpack.weaponType); // Loads a "WeaponMelee" from Resources.
            GameObject meleeWeapon = Instantiate(prefab, transform.position, transform.rotation) as GameObject;
            backpack.weapon = meleeWeapon;
            //TODO: SPECIFY THE WEAPON HOOK ETC
            meleeWeapon.transform.parent = transform; // attach the weapon to this gameObject 
        }
        if (combatStats == null)
            combatStats = this.gameObject.AddComponent<CombatStats>() as CombatStats;

        this.navAgent = this.gameObject.GetComponent<NavMeshAgent>();
    }

    void Update()
    {

    }

    /**
	 * Key-Value data that will feed the GOAP actions and system while planning.
	 */
    public HashSet<KeyValuePair<string, object>> getWorldState()
    {
        HashSet<KeyValuePair<string, object>> worldData = new HashSet<KeyValuePair<string, object>>();

        worldData.Add(new KeyValuePair<string, object>("hasPotion", (backpack.numPotions > 0)));
        worldData.Add(new KeyValuePair<string, object>("hasWeapon", (backpack.weapon != null)));

        worldData.Add(new KeyValuePair<string, object>("isHealthy", (combatStats.healthPoints > 50)));
        worldData.Add(new KeyValuePair<string, object>("isDisabled", (combatStats.isDisabled == true)));
        worldData.Add(new KeyValuePair<string, object>("isSlowed", (combatStats.isSlowed == true)));

        worldData.Add(new KeyValuePair<string, object>("isAware", (combatStats.isAware == true)));

        return worldData;
    }

    /**
	 * Implement in subclasses
	 */
    public abstract HashSet<KeyValuePair<string, object>> createGoalState();


    public void planFailed(HashSet<KeyValuePair<string, object>> failedGoal)
    {
        //TODO: FIGURE OUT PLAN FAIL


        // Not handling this here since we are making sure our goals will always succeed.
        // But normally you want to make sure the world state has changed before running
        // the same goal again, or else it will just fail.
    }

    public void planFound(HashSet<KeyValuePair<string, object>> goal, Queue<GoapAction> actions)
    {
        // Yay we found a plan for our goal
        Debug.Log("<color=green>Plan found</color> " + GoapAgent.prettyPrint(actions));
    }

    public void actionsFinished()
    {
        // Everything is done, we completed our actions for this goal. Hooray!
        Debug.Log("<color=blue>Actions completed</color>");
    }

    public void planAborted(GoapAction aborter)
    {
        // An action bailed out of the plan. State has been reset to plan again.
        // Take note of what happened and make sure if you run the same goal again
        // that it can succeed.
        Debug.Log("<color=red>Plan Aborted</color> " + GoapAgent.prettyPrint(aborter));
    }

    public bool moveAgent(GoapAction nextAction)
    {
        //TODO: Add moveSpeed && distance checkers per action.
        this.navAgent.SetDestination(nextAction.target.transform.position);

        float distance = (nextAction.target.transform.position - this.gameObject.transform.position).magnitude;

        if (distance <= 0.5f) //0.5 is good enough for melee classes like warrior
        {
            nextAction.setInRange(true);
            return true;
        }
        else
        {
            return false;
        }
    }
}