using Godot;
using System;

public class PlayerSM : StateMachine
{
	private Player parent;
	private WorldSM worldStates;

	public override void _Ready()
	{
        parent = GetParent<Player>();
		worldStates = GetNode<WorldSM>("/root/World/WorldSM");
		CallDeferred("setState", States.idle);
	}

	public override void _Input(InputEvent @event)
	{
		if (worldStates.gravity == WorldSM.GravityState.ON)
		{
			Jump(@event);
		}
	}
    private void Jump(InputEvent @event)
	{
		if(state == States.idle | state == States.running)
		{
			if (@event.IsActionPressed("ui_jump")) parent.velocity.y = (float)parent.JUMPSTRENGTH; //TODO: possible signal up
		}
		else if(state == States.jumping)
		{
			if (@event.IsActionReleased("ui_jump")) parent.velocity.y -= (float)(parent.JUMPSTRENGTH/3); //TODO: possible signal up
		}
	}

	public override void stateLogic(float delta)
	{
		parent.detectDirectionalInput(); //TODO: possible signal up
		parent.applyGravity(delta); //TODO: possible signal up
		parent.updatePlayerPosition(delta); //TODO: possible signal up
	}

	public override void exitState(States oldState, States newState)
	{
		return;
	}

	public override States getTransition(float delta)
	{
		switch (state)
		{
			case States.idle:
				if(!parent.IsOnFloor())
				{
					if(parent.velocity.y <0) return States.jumping; //TODO: possible signal up
					else if (parent.velocity.y > 0) return States.falling; //TODO: possible signal up
					else return state;
				}
				else if(parent.velocity.x !=0)
				{
					return States.running;
				}
				else return state;

			case States.running:
				if(!parent.IsOnFloor())
				{
					if(parent.velocity.y <0) return States.jumping; //TODO: possible signal up
					else if (parent.velocity.y > 0) return States.falling; //TODO: possible signal up
					else return state;
				}
				else if(parent.velocity.x == 0) //TODO: possible signal up
				{
					return States.idle;
				}
				else return state;

			case States.jumping:
				if(parent.IsOnFloor())  //TODO: possible signal up
				{
					return States.idle;
				}
				else if(parent.velocity.y >=0)  //TODO: possible signal up
				{
					return States.falling;
				}
				else return state;

			case States.falling:
				if(parent.IsOnFloor())  //TODO: possible signal up
				{
					return States.idle;
				}
				else if(parent.velocity.y <0)  //TODO: possible signal up
				{
					return States.jumping;
				}
				else return state;
			default:
				return state;
		}
	}

	public override void enterState(States newState, States oldState)
	{
		switch (state)
		{
			case States.idle:
				if (parent.horizontalDirection.x > 0){ 
					parent.animationPlayer.Play("IdleRight"); //TODO: possible signal up
				}
				else if (parent.horizontalDirection.x < 0){
					parent.animationPlayer.Play("IdleLeft"); //TODO: possible signal up
				}
				break;
			case States.running:
				if (parent.inputVector.x > 0){
					parent.animationPlayer.Play("RunRight"); //TODO: possible signal up
				}
				else if(parent.inputVector.x < 0){
					parent.animationPlayer.Play("RunLeft"); //TODO: possible signal up
				}
				break;
			case States.jumping:
				if (parent.inputVector.x > 0){
					parent.animationPlayer.Play("RunRight"); //FIXME: change to jump animation
				}
				else if(parent.inputVector.x < 0){
					parent.animationPlayer.Play("RunLeft"); //FIXME: change to jump animation
				}
				break;
			case States.falling:
				if (parent.inputVector.x > 0){
					parent.animationPlayer.Play("RunRight");  //FIXME: change to fall animation
				}
				else if(parent.inputVector.x < 0){
					parent.animationPlayer.Play("RunLeft");  //FIXME: change to fall animation
				}
				break;
		}
	}

}
