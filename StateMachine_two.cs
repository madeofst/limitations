using Godot;
using System;

public class StateMachine_two : Node2D
{
    public enum State 
    {
        none,
        idle,
        running,
        jumping,
        falling
    }

    private State state;
    private State previousState;

    public override void _Ready()
    {
        Node2D parent = GetParent<Node2D>();
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
        return State.none;
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
