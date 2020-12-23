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

		private State falling;
		private State floating;
		private State idle;
		private State jumping;
		private State running;
		private State pushing;
				
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

		public State getReplacement()
		{
			if (world.Gravity == World.GravityState.ON)
			{
				if (player.grounded)
				{
					if (player.velocity.x == 0)
					{
						return idle;
					}
					else
					{
						return running;
					}
				}
				else 
				{
					if (player.velocity.y < 0) 
					{
						return jumping;
					}
					else
					{
						return falling;
					}
				}
			}
			else
			{
				if (player.grounded)
				{
					return floating; //TODO: check for grabbing here
				}
				else
				{
					return floating;
				}
			}
		}

		public abstract void exitState();

		public override void _PhysicsProcess(float delta)
		{
			//intentionally empty. all code here will be inherited by states.
		}
	}
}
