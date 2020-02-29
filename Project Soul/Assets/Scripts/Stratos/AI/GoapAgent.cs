﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public sealed class GoapAgent : MonoBehaviour
{
    /*
     * One Job.
     * Create an experience.
     */

    //------GOAL GENERATOR-------
    //V1.1 vars
    private FSM stateMachine;

    private FSM.FSMState idleState; // finds something to do
    private FSM.FSMState moveToState; // moves to a target
    private FSM.FSMState performActionState; // performs an action - animation

    private HashSet<GoapAction> availableActions;
    private Queue<GoapAction> currentActions;

    private IGoap dataProvider; // this is the implementing class that provides our world data and listens to feedback on planning

    private GoapPlanner planner;


    //V1.2 vars
    public bool enableConsoleMessages = true;

    private int agentID; 
    


    void Start()
    {
        //NEXT LEVEL SHIT
        //agentID = GameObject.FindGameObjectWithTag("Manager").GetComponent<AI_Manager>().RegisterNewAgent(this.gameObject);
        agentID = UnityEngine.Random.Range(0, 100);

        stateMachine = new FSM();
        availableActions = new HashSet<GoapAction>();
        currentActions = new Queue<GoapAction>();
        planner = new GoapPlanner();
        findDataProvider(); //returns all components with IGOAP interface implemented
        createIdleState();
        createMoveToState();
        createPerformActionState();
        stateMachine.pushState(idleState);
        loadActions();
    }


    void Update()
    {
        //----------------------------------------------------------
        //TODO: TO RUN UPDATE METHOD CHECK IF ISANIMATING!!!!!!!!!!!
        //----------------------ASAP ASAP ASAP----------------------
        //----------------------------------------------------------
        stateMachine.Update(this.gameObject);
    }


    public void addAction(GoapAction a)
    {
        availableActions.Add(a);
    }

    public GoapAction getAction(Type action)
    {
        foreach (GoapAction g in availableActions)
        {
            if (g.GetType().Equals(action))
                return g;
        }
        return null;
    }

    public void removeAction(GoapAction action)
    {
        availableActions.Remove(action);
    }

    private bool hasActionPlan()
    {
        return currentActions.Count > 0;
    }

    private void createIdleState()
    {
        idleState = (fsm, gameObj) => {  //lambda expression saves the day
                                         // GOAP planning

            // get the world state and the goal we want to plan for
            HashSet<KeyValuePair<string, object>> worldState = dataProvider.getWorldState(); //Gets WorldState from BaseClass:InherritedClass script.
            HashSet<KeyValuePair<string, object>> goal = dataProvider.createGoalState(2);

            // Plan
            Queue<GoapAction> plan = planner.plan(gameObject, availableActions, worldState, goal);
            if (plan != null)
            {
                // we have a plan, hooray!
                currentActions = plan;
                dataProvider.planFound(goal, plan);

                fsm.popState(); // move to PerformAction state
                fsm.pushState(performActionState);

            }
            else
            {
                // ugh, we couldn't get a plan
                if (enableConsoleMessages)
                {
                    Debug.Log("<color=orange>Failed Plan:</color>" + prettyPrint(goal));
                }
                dataProvider.planFailed(goal);
                fsm.popState(); // move back to IdleAction state
                fsm.pushState(idleState);
            }
        };
    }

    private void createMoveToState()
    {
        moveToState = (fsm, gameObj) => {    //Lambda expression to refer actual method.
            // move the game object

            GoapAction action = currentActions.Peek();
            if (action.requiresInRange() && action.target == null)
            {
                Debug.Log("<color=red>Fatal error:</color> Action requires a target but has none. Planning failed. You did not assign the target in your Action.checkProceduralPrecondition()");
                fsm.popState(); // move
                fsm.popState(); // perform
                fsm.pushState(idleState);
                return;
            }

            // get the agent to move itself
            if (dataProvider.moveAgent(action))
            {
                fsm.popState();
            }

            /*MovableComponent movable = (MovableComponent) gameObj.GetComponent(typeof(MovableComponent));
			if (movable == null) {
				Debug.Log("<color=red>Fatal error:</color> Trying to move an Agent that doesn't have a MovableComponent. Please give it one.");
				fsm.popState(); // move
				fsm.popState(); // perform
				fsm.pushState(idleState);
				return;
			}

			float step = movable.moveSpeed * Time.deltaTime;
			gameObj.transform.position = Vector3.MoveTowards(gameObj.transform.position, action.target.transform.position, step);

			if (gameObj.transform.position.Equals(action.target.transform.position) ) {
				// we are at the target location, we are done
				action.setInRange(true);
				fsm.popState();
			}*/
        };
    }

    private void createPerformActionState()
    {

        performActionState = (fsm, gameObj) => {
            // perform the action

            if (!hasActionPlan())
            {
                // no actions to perform
                Debug.Log("<color=red>Done actions</color>");
                fsm.popState();
                fsm.pushState(idleState);
                dataProvider.actionsFinished();
                return;
            }

            GoapAction action = currentActions.Peek();
            if (action.isDone())
            {
                // the action is done. Remove it so we can perform the next one
                currentActions.Dequeue();
            }

            if (hasActionPlan())
            {
                // perform the next action
                action = currentActions.Peek();
                bool inRange = action.requiresInRange() ? action.isInRange() : true;
                //ternary conditional operator, evaluates a Boolean expression and returns the result of one of the two expressions

                if (inRange)
                {
                    // we are in range, so perform the action
                    bool success = action.perform(gameObj);

                    if (!success)
                    {
                        // action failed, we need to plan again
                        fsm.popState();
                        fsm.pushState(idleState);
                        dataProvider.planAborted(action);
                    }
                }
                else
                {
                    // we need to move there first
                    // push moveTo state
                    fsm.pushState(moveToState);
                }
            }
            else
            {
                // no actions left, move to Plan state
                fsm.popState();
                fsm.pushState(idleState);
                dataProvider.actionsFinished();
            }

        };
    }

    private void findDataProvider()
    {
        foreach (Component comp in gameObject.GetComponents(typeof(Component)))
        {
            //Type.IsAssignableFrom = Determines whether an instance of a specified type can be assigned to a variable of the current type.
            if (typeof(IGoap).IsAssignableFrom(comp.GetType()))
            {
                dataProvider = (IGoap)comp;
                return;
            }
        }
    }

    private void loadActions()
    {
        GoapAction[] actions = gameObject.GetComponents<GoapAction>();
        foreach (GoapAction a in actions)
        {
            availableActions.Add(a);
        }
        if (enableConsoleMessages)
        {
            Debug.Log("Found actions on agent " + agentID + ": " + prettyPrint(actions));
        }
    }

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
}
