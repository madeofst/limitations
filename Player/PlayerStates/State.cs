using Godot;
using System;

namespace Limitations
{
	public abstract class State : Node2D
	{
		public AnimationPlayer animationPlayer { get; set; }
		public PlayerStateMachine playerStateMachine { get; set; }
		public Player player { get; set; }
		public World world { get; set; }

		public State falling { get; set; }
		public State floating { get; set; }
		public State idle { get; set; }
		public State jumping { get; set; }
		public State running { get; set; }
		public State pushing { get; set; }
				
		public override void _Ready()
		{
			world = GetNode<World>("/root/World");
			player = world.GetNode<Player>("Player");
			animationPlayer = player.GetNode<AnimationPlayer>("AnimationPlayer");
			
			falling = player.GetNode<State>("PlayerStateMachine/State/Falling");
			floating = player.GetNode<State>("PlayerStateMachine/State/Floating");
			idle = player.GetNode<State>("PlayerStateMachine/State/Idle");
			jumping = player.GetNode<State>("PlayerStateMachine/State/Jumping");
			running = player.GetNode<State>("PlayerStateMachine/State/Running");
			pushing = player.GetNode<State>("PlayerStateMachine/State/Pushing");
		}

		public virtual void enterState()
		{ 
		}

		public virtual State getReplacement()
		{
			if (world.Gravity == World.GravityState.OFF)
			{
				return floating;
			}
			else
			{
				return null;
			}
		}

		public abstract void exitState();

		public override void _PhysicsProcess(float delta)
		{
			//intentionally empty. all code here will be inherited by states.
		}
	}
}
