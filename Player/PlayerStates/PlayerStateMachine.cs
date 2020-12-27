using Godot;
using Limitations;
using System;

public class PlayerStateMachine : Node2D
{
	public Player _player { get; set; }
	public World _world { get; set; }
	public State currentState { get; set; }

	public override void _Ready()
	{
		_player = GetNode<Player>("..");
		_world = GetNode<World>("/root/World");
		CallDeferred("SetState",GetNode<State>("State/Idle"));
	}

	public void SetState(State newState)
	{
		State previousState;
		previousState = currentState;
		currentState = newState;
		if (previousState!=null) previousState.exitState();      //TODO: This needs additional checks
		if (newState!=null) 
		{
			GD.Print(currentState.Name);
			currentState.enterState();
		}
	}

	public void ChangeState()
	{
		State newState;
		newState = currentState.getReplacement();
		if (currentState != null && newState != null)
		{		
			if (newState != currentState) GD.Print(newState.Name);
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
		//TODO: look at signals here
		_player.detectDirectionalInput();
		_player.CheckSurfaceCollisions();
		_player.applyGravity(delta);
		_player.updateHorizontalPlayerPosition(delta);
		_player.setPlayerDirection();
		_player.CheckSurfaceCollisions();
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_jump"))
		{
			if (currentState is Idle | currentState is Running)
			{
				SetState(GetNode<State>("State/Jumping"));
				_player.velocity.y = (float)_player.jumpStrength; //TODO: possible signal up
			}
		}
		
		if (@event.IsActionReleased("ui_jump"))
		{	
			if (currentState is Jumping)
			{
				_player.velocity.y -= (float)(_player.jumpStrength / 3); //TODO: possible signal up
			}
		}
	}
}
