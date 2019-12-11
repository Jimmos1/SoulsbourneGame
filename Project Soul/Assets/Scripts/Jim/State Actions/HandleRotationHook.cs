using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleRotationHook : StateAction
{
    PlayerStateManager states;
    MovePlayerCharacter move;

    public HandleRotationHook(PlayerStateManager states, MovePlayerCharacter move)
    {
        this.states = states;
        this.move = move;
    }

    public override bool Execute()
    {
        if (states.canRotate)
        {
            move.HandleRotation();
        }

        return false;
    }
}
