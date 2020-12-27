using Godot;
using Limitations;
using System;

public class Player : KinematicBody2D
{
	public int friction { get; set; }
	public int gravityStrength { get; set; }
	public int accleration { get; set; }
	public int maxSpeed { get; set; } = 225;
	public int terminalVelocity { get; set; } = 500; 
	public int jumpStrength { get; set; } //TODO: Look at using properties for these
	public int inertia { get; set; } = 300;
	public float drag { get; set; } = 0.5f;
	public int playerGravityMultiplier { get; set; } = 1;

	public Vector2 inputVector = new Vector2(Vector2.Zero);
	public Vector2 playerDirection = new Vector2(Vector2.Right);
	public Vector2 velocity = new Vector2(0, 50);
	
	public World world { get; private set; }
	private RayCast2D directionalRay;

	public bool onFloor { get; set; }
	public bool onLeftWall { get; set; }
	public bool onRightWall { get; set; }
	public bool onCeiling { get; set; }
	public bool onObject { get; set; }
	public bool grounded { get; set; }

	public override void _Ready()
	{ 
		world = GetNode<World>("/root/World");
		directionalRay = GetNode<RayCast2D>("RayCastVertical/RayCast2D");
	}

	public void detectDirectionalInput()
	{
		inputVector.y = Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up");
		inputVector.x = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left"); 
	}

	public void CheckSurfaceCollisions()
	{
		resetSurfaceCollisions();
		Node2D rayCasts = GetNode<Node2D>("RayCasts");
		for (int i=0; i<=rayCasts.GetChildCount()-1; i++)
		{
			RayCasts ray = rayCasts.GetChild<RayCasts>(i);
			ray.checkCollisions();
		}
		//printInteractionStatus();
	}

    private void resetSurfaceCollisions()
    {	
		grounded = false;
		onObject = false;
		onFloor = false;
		onLeftWall = false;
		onRightWall = false;
 		onCeiling = false;
    }

    public void applyGravity(float delta)
	{
		if (!grounded)
		{
			velocity.y += delta * gravityStrength;
		}
	}

	public void updateHorizontalPlayerPosition(float delta)
	{
		if (world.Gravity == World.GravityState.ON)
		{
			Vector2 horizontalVelocity = new Vector2(velocity.x, 0);
			if (inputVector.x != 0)
			{
				velocity.x = horizontalVelocity.MoveToward(inputVector * maxSpeed, delta * accleration).x;
			}
			else if (grounded)
			{
				velocity.x = horizontalVelocity.MoveToward(Vector2.Zero, friction * delta).x;
			}
			else
			{
				velocity.x = horizontalVelocity.MoveToward(Vector2.Zero, drag * velocity.x * delta).x;
			}

			if (onLeftWall && inputVector.x == 0 && velocity.x < 0)
			{ 
				velocity.x = 0;
			}
			else if (onRightWall && inputVector.x == 0 && velocity.x > 0)
			{
				velocity.x = 0;
			}
		}

		KinematicCollision2D collision = MoveAndCollide(velocity * delta, false, true, false);

		//CheckSurfaceCollisions();

		if (collision != null)
		{
			if (world.Gravity == World.GravityState.ON)
			{
				if (collision.Collider is RigidBody2D && grounded)
				{
					PlayerStateMachine statemachine = GetNode<PlayerStateMachine>("PlayerStateMachine");
					if (statemachine.currentState != statemachine.currentState.pushing) 
					{
						statemachine.SetState(statemachine.GetNode<State>("State/Pushing"));
					}

					RigidBody2D rigidBody = collision.Collider as RigidBody2D;
					rigidBody.ApplyCentralImpulse(-collision.Normal * inertia);
					velocity = collision.Remainder.Slide(Vector2.Up);
				}
				else
				{
					velocity = velocity.Slide(collision.Normal);
				}
			}
			else
			{
				var reflect = collision.Remainder.Bounce(collision.Normal);
				velocity = velocity.Bounce(collision.Normal);
				MoveAndCollide(reflect);
			}
		}
	}   

	public void setPlayerDirection()
	 {
		if (velocity == Vector2.Zero)
		{
			if (inputVector.y != 0)
			{
				playerDirection.y = inputVector.y;
			}
			else
			{
				playerDirection.y = 0;
			}

			if (inputVector.x != 0)
			{
				playerDirection.x = inputVector.x;
			}
		}
		else
		{
			playerDirection = velocity;
		}
		directionalRay.Rotation = Mathf.Atan2(playerDirection.y, playerDirection.x);
	}

	private String previousInteractions;
	private void printInteractionStatus()
	{
		String interactions = "";
		if (onObject)
		{
			interactions += ", onObject";
		}
		if (onFloor)
		{
			interactions += ", onFloor";
		}
		if (onCeiling)
		{
			interactions += ", onCeiling";
		}
		if (onLeftWall)
		{
			interactions += ", onLeftWall";
		}
		if (onRightWall)
		{
			interactions += ", onRightWall";
		}
		if (grounded)
		{
			interactions += ", grounded";
		}
		if (interactions.Length != 0)
		{
			interactions.TrimStart(',');
			if (!interactions.Equals(previousInteractions))
			{
				GD.Print(interactions);
			}
			previousInteractions = interactions;
		}
	}
	public void printParameters()
	{
		GD.Print($"FRICTION={friction}");
		GD.Print($"GRAVITYSTRENGTH={gravityStrength}");
		GD.Print($"ACCELERATION={accleration}");
		GD.Print($"MAXSPEED={maxSpeed}");
		GD.Print($"TERMINALVELOCITY={terminalVelocity}");
		GD.Print($"JUMPSTRENGTH={jumpStrength}");
		GD.Print($"INERTIA={inertia}");
	}

}
