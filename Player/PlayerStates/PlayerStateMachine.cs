using Godot;
using Limitations;
using System;

public class PlayerStateMachine : Node2D
{
    private State currentState;
    private Player parent;
    private World world;

    public Player Parent { get => parent; set => parent = value; }

    public override void _Ready()
	{
        Parent = GetParent<Player>();
        world = GetNode<World>("/root/World");
        CallDeferred("SetState", new Idle(Parent));
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
            //GD.Print(currentState.Name);
        }
    }

    public virtual void stateLogic(float delta)
    {
        Parent.detectDirectionalInput(); //TODO: possible signal up
        Parent.checkAllCollisions();
		Parent.applyGravity(delta); //TODO: possible signal up
		Parent.updateHorizontalPlayerPosition(delta); //TODO: possible signal up
        Parent.checkAllCollisions();
        Parent.setPlayerDirection();
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
            Parent.velocity.y = (float)Parent.JUMPSTRENGTH; //TODO: possible signal up
        }
        else if (currentState is Jumping)
        {
            Parent.velocity.y -= (float)(Parent.JUMPSTRENGTH / 3); //TODO: possible signal up
        }
    }
}
