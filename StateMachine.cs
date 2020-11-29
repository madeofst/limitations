using Godot;
using System;

public class StateMachine : Node2D
{
    public enum States 
    {
        none,
        idle,
        running,
        jumping,
        falling
    }

    public States state;
    public States previousState;

    public override void _PhysicsProcess(float delta)
    {
        if (state!=States.none)
        {
            stateLogic(delta);
            States transition = getTransition(delta);
            if (transition != States.none) setState(transition);
            //GD.Print(state);
        }
    }

    public void setState(States newState)
    {
        previousState = state;
        state = newState;
        if(previousState!=States.none) exitState(previousState,newState);
        if(newState!=States.none) enterState(newState,previousState);
    }

    public virtual void stateLogic(float delta)
    {
    }

    public virtual States getTransition(float delta)
    {
        return state;
    }

    public virtual void enterState(States newState, States oldState)
    {
    }

    public virtual void exitState(States oldState, States newState)
    {
    }

}
