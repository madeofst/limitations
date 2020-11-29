using Godot;
using System;

public class StateMachine : Node2D
{
    public enum State 
    {
        none,
        idle,
        running,
        jumping,
        falling
    }

    public State state;
    public State previousState;
    public Player parent;

    public override void _Ready()
    {
        parent = GetParent<Player>();
    }

    public override void _PhysicsProcess(float delta)
    {
        if (state!=State.none)
        {
            stateLogic(delta);
            State transition = getTransition(delta);
            if (transition != State.none) setState(transition);
        }
    }

    public void setState(State newState)
    {
        previousState = state;
        state = newState;
        if(previousState!=State.none) exitState(previousState,newState);
        if(newState!=State.none) enterState(newState,previousState);
    }

    public virtual void stateLogic(float delta)
    {
        return;
    }

    public virtual State getTransition(float delta)
    {
        return state;
    }

    public virtual void enterState(State newState, State oldState)
    {
        return;
    }

    public virtual void exitState(State oldState, State newState)
    {
        return;
    }




}
