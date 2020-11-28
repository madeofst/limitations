using Godot;
using System;

public class PlayerSM : StateMachine
{

    public override void stateLogic(float delta)
    {
        return;
    }

    public override State getTransition(float delta)
    {
        return State.none;
    }

    public override void enterState(State newState, State oldState)
    {
        return;
    }

    public override void exitState(State oldState, State newState)
    {
        return;
    }

}
