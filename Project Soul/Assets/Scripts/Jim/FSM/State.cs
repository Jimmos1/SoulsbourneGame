using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    bool forceExit;
    List<StateAction> fixedUpdateActions = new List<StateAction>();
    List<StateAction> updateActions = new List<StateAction>();
    List<StateAction> lateUpdateActions = new List<StateAction>();

    public delegate void OnEnter();
    public OnEnter onEnter;
    
    //Constructor
    public State(List<StateAction> fixedUpdateActions, List<StateAction> updateActions, List<StateAction> lateUpdateActions)
    {
        this.fixedUpdateActions = fixedUpdateActions;
        this.updateActions = updateActions;
        this.lateUpdateActions = lateUpdateActions;
    }

    public void FixedTick()
    {
        ExecuteListOfActions(fixedUpdateActions);
    }

    public void Tick()
    {
        ExecuteListOfActions(updateActions);

    }

    public void LateTick()
    {
        ExecuteListOfActions(lateUpdateActions);

        forceExit = false;
    }

    void ExecuteListOfActions(List<StateAction> l)
    {
        for (int i = 0; i < l.Count; i++)
        {
            if (forceExit)
                return;

            forceExit = l[i].Execute();
        }
    }
}
