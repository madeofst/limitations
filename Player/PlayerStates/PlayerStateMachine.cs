using Godot;
using Limitations;
using System;

public class PlayerStateMachine : Node2D
{
	public Player _player { get; set; }
	public World _world { get; set; }
	private State currentState;

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
		//TODO: look at signals here
		_player.detectDirectionalInput();
		_player.CheckSurfaceCollisions();
		_player.applyGravity(delta);
		_player.updateHorizontalPlayerPosition(delta);
		_player.setPlayerDirection();
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
			_player.velocity.y = (float)_player.jumpStrength; //TODO: possible signal up
		}
		else if (currentState is Jumping)
		{
			_player.velocity.y -= (float)(_player.jumpStrength / 3); //TODO: possible signal up
		}
	}
}
