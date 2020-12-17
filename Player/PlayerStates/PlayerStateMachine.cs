using Godot;
using Limitations;
using System;

public class PlayerStateMachine : Node2D
{
    private State currentState;
    private Player parent;

    public override void _Ready()
	{
        parent = GetParent<Player>();
        CallDeferred("SetState", new Idle(parent));
    }

    public void SetState(State newState)
    {
        State previousState;
        previousState = currentState;
        currentState = newState;
        if (previousState!=null) previousState.exitState();      //TODO: This needs additional checks
        if (newState!=null) currentState.enterState();
    }

    public void ChangeState()
    {
        State newState;
        newState = currentState.getReplacement();
        if (currentState != null && newState != null)
        {
            currentState.exitState();
            newState.enterState();
            currentState = newState;
        }
    }
 
    public override void _PhysicsProcess(float delta)
    {
        if (currentState != null) 
        {
            stateLogic(delta);
            ChangeState();
        }
    }

    public virtual void stateLogic(float delta)
    {
        parent.detectDirectionalInput(); //TODO: possible signal up
		parent.applyGravity(delta); //TODO: possible signal up
		parent.updatePlayerPosition(delta); //TODO: possible signal up
        parent.setPlayerDirection();
    }

    public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_jump"))
        {
            Jump(@event);
        }
	}

    private void Jump(InputEvent @event)
    {
        if (currentState is Idle | currentState is Running)
        {
            parent.velocity.y = (float)parent.JUMPSTRENGTH; //TODO: possible signal up
        }
        else if (currentState is Jumping)
        {
            parent.velocity.y -= (float)(parent.JUMPSTRENGTH / 3); //TODO: possible signal up
        }
    }
}
