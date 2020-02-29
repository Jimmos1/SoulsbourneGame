using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

//V2.1
//FINAL VERSION FOR PROJECT SOUL (Current build is Alpha) 
public sealed class GoapCore : MonoBehaviour, ILockable, IDamageable, IDamageEntity, IParryable 
{
    /*
     * One Job.
     * Create an experience.
     */

    //Tactical vars
    private FSM stateMachine;        //The state machine that will manage our AI behaviour cycle.
    private FSM.FSMState idleState;       
    private FSM.FSMState moveToState;
    private FSM.FSMState performActionState;

    private HashSet<GoapAction> availableActions; //Action pool. This is fully extendable.
    private Queue<GoapAction> currentActions;     //Current action queue.

    private IGoap dataProvider;  //Current AI class that provides world data and listens to feedback on planning.
    private GoapPlanner planner;
    private int goalGeneratorID = 1;

    //Animation vars
    new Rigidbody rigidbody;
    Animator animator;
    NavMeshAgent agent;
    AnimatorHook animatorHook;
    Transform mTransform;
    Vector3 lookPosition;

    public float rotationSpeed = 1;
    public float moveSpeed = 2;
    
    public bool isInInterruption; //Disruption mechanics (e.g. backstab and parry)
    public bool openToBackstab = true;
    bool isInteracting;  //Performing any action
    bool actionFlag;
    public float recoveryTimer;
    public float parriedDistance = 1.5f;

    //REMOVE ASAP
    Controller currentTarget;
    public ActionSnapshot[] actionSnapshots;
    ActionSnapshot currentSnapshot;
    ActionContainer _lastAction; //TODO: Read ActionSnapshot and Container and merge this logic with GOAP.

    //Perception vars
    public float fovRadius = 5; //TODO: Testing.
    private LayerMask detectionLayer;
    public int targetLayer = 8; //8th layer is the player.

    //Combat vars
    public int health = 100;
    private bool isHit; //TODO: Change in future versions (public maybe)
    private float hitTimer;
    public GameObject damageCollider; //The collider we enable/disable to deal damage.


    //Helper vars
    private int agentID;
    public bool enableConsoleMessages = true;
    [HideInInspector]
    public Transform lockOnTarget; //TODO: make private
    
    void Start()
    {
        //setting up refs
        detectionLayer = (1 << targetLayer); //Setting layer mask for player detection
        mTransform = this.transform;
        rigidbody = GetComponentInChildren<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        agent = GetComponentInChildren<NavMeshAgent>();
        rigidbody.isKinematic = false;
        animatorHook = GetComponentInChildren<AnimatorHook>();

        //NEXT LEVEL SHIT
        //agentID = GameObject.FindGameObjectWithTag("Manager").GetComponent<AI_Manager>().RegisterNewAgent(this.gameObject);
        agentID = UnityEngine.Random.Range(0, 100);
        stateMachine = new FSM();
        availableActions = new HashSet<GoapAction>();
        currentActions = new Queue<GoapAction>();
        planner = new GoapPlanner();

        FindDataProvider(); //Assigns a script as data provider for our AI.
        createIdleState();
        createMoveToState();
        createPerformActionState();

        stateMachine.pushState(idleState);
        loadActions();
        Debug.Log("Agent Init");
        //loadActions
        
    }
    private void Update()
    {
        float delta = Time.deltaTime;

        isInInterruption = animator.GetBool("interrupted");
        isInteracting = animator.GetBool("isInteracting");

        if (isHit)  //Invincible          ---TODO: Make source specific.---
        {
            if(hitTimer > 0)
            {
                hitTimer -= delta;
            }
            else
            {
                //ready to take damage again
                isHit = false;
            }
        }

        if (currentTarget == null) //means is NOT aware of player - do default goap goal here
        {
            //TODO: if dist from spawn > range threshold -> return to spawn (and reset)

            HandleDetection();

            stateMachine.Update(this.gameObject);

            //anim stuff
        }
        else              //means IS aware of player 
        {
            stateMachine.Update(this.gameObject);
            
            //HeadIK
            lookPosition = currentTarget.mTransform.position;
            lookPosition.y += 1.2f;
            animatorHook.lookAtPosition = lookPosition;

            //Move with Root Motion 
            Vector3 targetVel = animatorHook.deltaPosition * moveSpeed;
            rigidbody.velocity = targetVel;
        }
    }

    void LateUpdate()
    {
        //Validating agent
        agent.transform.localPosition = Vector3.zero;
        agent.transform.localRotation = Quaternion.identity;
    }

    private void FindDataProvider()  //Looks for a component with IGoap implemented, assigns it as dataProvider and then returns.
    {
        foreach(Component comp in gameObject.GetComponents(typeof(Component)))
        {
            //Type.IsAssignableFrom = Determines whether an instance of a specified type can be assigned to a variable of the current type.
            if (typeof(IGoap).IsAssignableFrom(comp.GetType()))
            {
                dataProvider = (IGoap)comp;
                return;
            }
        }
    }
    private void createIdleState() //Idle state refers to GOAP planning state.
    {
        idleState = (fsm, gameObj) => {   //lambda expression saves the day
                                          
                                          // get the world state and the goal we want to plan for
            HashSet<KeyValuePair<string, object>> worldState = dataProvider.getWorldState(); //Gets WorldState from BaseClass:InherritedClass script.
            HashSet<KeyValuePair<string, object>> goal = dataProvider.createGoalState(goalGeneratorID);

            //Plan
            Queue<GoapAction> plan = planner.plan(gameObject, availableActions, worldState, goal);
            if(plan != null)
            {
                //we have a plan, hooray!
                currentActions = plan;
                dataProvider.planFound(goal, plan);

                fsm.popState(); //move to PerformAction state
                fsm.pushState(performActionState);
            }
            else
            {
                //ugh, we couldn't get a plan
                if (enableConsoleMessages)
                {
                    Debug.Log("<color=orange>Failed Plan:</color>" + prettyPrint(goal));
                }
                dataProvider.planFailed(goal);
                fsm.popState();    //move back to IdleAction state
                fsm.pushState(idleState);
            }
        };
    }

    private void createMoveToState() //Move state refers to GOAP current action target pathfinding.
    {
        moveToState = (fsm, gameobj) => {

            GoapAction action = currentActions.Peek();
            if(action.requiresInRange() && action.target == null)
            {
                if (enableConsoleMessages)
                {
                    Debug.Log("<color=red>Fatal error:</color> Action requires a target but has none. Planning failed." +
                        "You did not assign the target in your Action.checkProceduralPrecondition()");
                }
                fsm.popState(); //remove move
                fsm.popState(); //remove perform
                fsm.pushState(idleState); //try again rtrd
                return;
            }

            //Tell the archetype class to move the agent
            //Returns true only when we are on current action target position.
            if (dataProvider.moveAgent(action))
            {
                fsm.popState();
            }
        };
    }

    private void createPerformActionState() //Perform state refers to GOAP current action target pathfinding.
    {
        performActionState = (fsm, gameObj) =>
        {
            if (!hasActionPlan())
            {
                //no actions to perform
                if (enableConsoleMessages)
                {
                    Debug.Log("<color=red>Done actions</color>");
                }
                fsm.popState();
                fsm.pushState(idleState); //reset and re-plan
                dataProvider.actionsFinished();
                return;
            }

            GoapAction action = currentActions.Peek();
            if (action.isDone())
            {
                //the action is done. Remove it so we can perform the next one
                currentActions.Dequeue();
            }

            if (hasActionPlan())
            {
                //perform the next action
                action = currentActions.Peek();

                //ternary conditional operator, evaluates a Boolean expression and returns the result of one of the two expressions
                bool inRange = action.requiresInRange() ? action.isInRange() : true;

                if (inRange)
                {
                    //we are in range, so perform the action
                    bool success = action.perform(gameObj);

                    if (!success)
                    {
                        //action failed, we need to plan again
                        fsm.popState();
                        fsm.pushState(idleState);
                        dataProvider.planAborted(action);
                    }
                }
                else
                {
                    //we need to move there first 
                    //push moveTo state
                    fsm.pushState(moveToState);
                }
            }
            else
            {
                //no actions left, move to idle state for new plan
                fsm.popState();
                fsm.pushState(idleState);
                dataProvider.actionsFinished();
            }
        };
    }
    private void loadActions() //Creating a pool of current actions from this AI.
    {
        GoapAction[] actions = gameObject.GetComponentsInChildren<GoapAction>(); //TODO: Reference child that contains the actions
        foreach (GoapAction a in actions)
        {
            availableActions.Add(a);
        }
        if (enableConsoleMessages)
        {
            Debug.Log("Found actions on agent " + agentID + ": " + prettyPrint(actions));
        }
    }
    private bool hasActionPlan()
    {
        return currentActions.Count > 0;
    }

    /*
     * The most important method is Play Target Animation because all actions end up here.
     * GOAP Actions ARE able to call it directly.
     */
    public void PlayTargetAnimation(string targetAnim, bool toBeInteracting, float crossfadeTime = 0.2f, bool playInstantly = false)
    {
        animator.SetBool("isInteracting", toBeInteracting);

        if (!playInstantly)
        {
            animator.CrossFadeInFixedTime(targetAnim, crossfadeTime);
        }
        else
        {
            animator.Play(targetAnim);
        }
    }
    public void HandleRotation(float delta, GameObject target)
    {
        Vector3 dir = target.transform.position - mTransform.position;
        dir.y = 0;
        dir.Normalize();

        if (dir == Vector3.zero)
        {
            dir = mTransform.forward;
        }

        float angle = Vector3.Angle(dir, mTransform.forward);
        if (angle > 5f)
        {
            animator.SetFloat("sideways", Vector3.Dot(dir, mTransform.right), 0.1f, delta);
        }
        else
        {
            animator.SetFloat("sideways", 0f, 0.1f, delta);
        }

        Quaternion targetRot = Quaternion.LookRotation(dir);
        mTransform.rotation = Quaternion.Slerp(mTransform.rotation, targetRot, delta / rotationSpeed);
    }

    void HandleDetection()
    {
        Collider[] cols = Physics.OverlapSphere(mTransform.position, fovRadius, detectionLayer);

        for (int i = 0; i < cols.Length; i++)
        {
            Controller controller = cols[i].transform.GetComponentInParent<Controller>();
            
            if (controller != null)
            {
                //TODO: LINECAST FOR WALLS
                IsAware(controller);
                return;
            }
        }
    }
    void IsAware(Controller target)
    {
        currentTarget = target;
        animatorHook.hasLookAtTarget = true;

        goalGeneratorID = 2;

        //TODO: TEST THIS OUT
        stateMachine.popState();
        stateMachine.pushState(idleState);
    }

    #region ILockable
    public bool IsAlive()
    {
        return health > 0;
    }

    public Transform GetLockOnTarget(Transform from)
    {
        return lockOnTarget;
    }
    #endregion ILockable 

    //TODO: IDamageable fix! Defensive ->Draw defensive stats here
    public void OnDamage(ActionContainer action)
    {
        if (action.owner == mTransform)
            return;

        if (!isHit)
        {
            isHit = true;
            hitTimer = 1f;
            health -= action.damage;
            animatorHook.CloseDamageCollider(); //for safety


            if (health <= 0)
            {
                PlayTargetAnimation("Death", true);
                animator.transform.parent = null; // in order for ragdoll to properly work
                gameObject.SetActive(false); // could just destroy instead of disabling
            }
            else
            {
                Vector3 direction = action.owner.position - mTransform.position;
                float dot = Vector3.Dot(mTransform.forward, direction);

                if (action.overrideReactAnim)
                {
                    PlayTargetAnimation(action.reactAnim, true);
                }
                else
                {
                    if (dot > 0)
                    {
                        PlayTargetAnimation("Get Hit Front", true, 0f, true);
                    }
                    else
                    {
                        PlayTargetAnimation("Get Hit Back", true, 0f, true);
                    }
                }
            }
        }
    }

    //TODO: IDamageEntity fix! Offensive ->Draw offensive stats here
    public ActionContainer GetActionContainer() //Gets current action to deal damage with it TODO: IMPLEMENT GOAP LOGIC
    {
        return GetLastAction;
    }
    public ActionContainer GetLastAction
    {
        get
        {
            if (_lastAction == null)
            {
                _lastAction = new ActionContainer();
            }

            _lastAction.owner = mTransform; //For directional attacks
            _lastAction.damage = currentSnapshot.damage;
            _lastAction.overrideReactAnim = currentSnapshot.overrideReactAnim;
            _lastAction.reactAnim = currentSnapshot.reactAnim;

            return _lastAction;
        }
    }

    #region IParryable
    public void OnParried(Vector3 dir)
    {
        if (animatorHook.canBeParried && tag != "Dragon") //dragon doesn't have animations
        {
            if (!isInInterruption)
            {
                animatorHook.CloseDamageCollider(); //for safety

                dir.Normalize(); // to rotate agent to look at us
                dir.y = 0;
                mTransform.rotation = Quaternion.LookRotation(dir);

                PlayTargetAnimation("Attack Interrupt", true, 0f, true);
            }
        }
    }

    public void GetParried(Vector3 origin, Vector3 direction)
    {
        mTransform.position = origin + direction * parriedDistance;
        mTransform.rotation = Quaternion.LookRotation(-direction);
        PlayTargetAnimation("Getting Parried", true, 0f, true);
        //TODO: Damage Calculations & Sounds
        //ALSO FORCE AI TO MAKE NEW PLAN
    }
    public void GetBackstabbed(Vector3 origin, Vector3 direction)
    {
        mTransform.position = origin + direction * parriedDistance;
        mTransform.rotation = Quaternion.LookRotation(direction);
        PlayTargetAnimation("Getting Backstabbed", true, 0f, true);
        //TODO: Damage Calculations & Sounds
        //ALSO FORCE AI TO MAKE NEW PLAN
    }
    public Transform getTransform() //Useful for dir calculations.
    {
        return mTransform;
    }
    public bool canBeParried()
    {
        return isInInterruption;
    }
    public bool canBeBackstabbed()
    {
        return openToBackstab; //Always true , this one depends on the type of AI we implement. Monster type of enemies might have this off.
    }
    #endregion
    //TODO: Generic AI methods
    public bool Reset()
    {
        return false;
    }
    public void Init()
    {

    }
    public void Despawn()
    {

    }
    public void Respawn()
    {

    }
    //HELPER METHODS
    public static string prettyPrint(HashSet<KeyValuePair<string, object>> state)
    {
        String s = "";
        foreach (KeyValuePair<string, object> kvp in state)
        {
            s += kvp.Key + ":" + kvp.Value.ToString();
            s += ", ";
        }
        return s;
    }
    public static string prettyPrint(GoapAction[] actions)
    {
        String s = "";
        foreach (GoapAction a in actions)
        {
            s += a.GetType().Name;
            s += ", ";
        }
        return s;
    }
    public static string prettyPrint(GoapAction action)
    {
        String s = "" + action.GetType().Name;
        return s;
    }
    public static string prettyPrint(Queue<GoapAction> actions)
    {
        String s = "";
        foreach (GoapAction a in actions)
        {
            s += a.GetType().Name;
            s += "-> ";
        }
        s += "GOAL";
        return s;
    }
}
