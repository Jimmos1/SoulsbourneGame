using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateNewGame : IState
{
    GameManager owner;

    public StateNewGame(GameManager owner) { this.owner = owner; }

    //POPULATE THE NEW STATE

    public void Enter()
    {
        throw new System.NotImplementedException();
    }

    public void Execute()
    {
        throw new System.NotImplementedException();
    }

    public void Exit()
    {
        throw new System.NotImplementedException();
    }
}
