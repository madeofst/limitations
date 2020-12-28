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
	public int inertia { get; set; } = 130;
	public float drag { get; set; } = 0.5f;
	public int playerGravityMultiplier { get; set; } = 1;

	public Vector2 inputVector = new Vector2(Vector2.Zero);
	public Vector2 playerDirection = new Vector2(Vector2.Right);
	public Vector2 velocity = new Vector2(0, 50);
	
	public World world { get; private set; }
	private RayCast2D directionalRay;
	private RichTextLabel label;

	public bool grounded { get; set; }

	public bool onLeftWall { get; set; }
	public bool onRightWall { get; set; }

	public bool onFloor { get; set; }
	public bool wallInFront { get; set; }
	public bool onCeiling { get; set; }

	public bool onObject { get; set; }
	public bool objectInFront { get; set; }


	public override void _Ready()
	{ 
		world = GetNode<World>("/root/World");
		directionalRay = GetNode<RayCast2D>("RayCastVertical/RayCast2D");
		label = GetNode<RichTextLabel>("RichTextLabel");
	}

	public void detectDirectionalInput()
	{
		inputVector.y = Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up");
		inputVector.x = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
		//inputVector.x = -1; 
	}

    private void resetSurfaceIndicators()
    {	
		grounded = false;

		onLeftWall = false;
		onRightWall = false;

		onFloor = false;
		wallInFront = false;
		onCeiling = false;
		
		onObject = false;
		objectInFront = false;
    }

	public void CheckCollisionsAndUpdateSurfaceIndicators()
	{
		resetSurfaceIndicators();
		Node2D rayCasts = GetNode<Node2D>("RayCasts");
		for (int i=0; i<=rayCasts.GetChildCount()-1; i++)
		{
			RayCasts ray = rayCasts.GetChild<RayCasts>(i);
			ray.checkCollisions();
		}
	}

    public void applyGravity(float delta)
	{
		if (!grounded)
		{
			velocity.y += delta * gravityStrength;
		}
	}

	public void updatePlayerPositionAndCollide(float delta)
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

			if (wallInFront)
			{
				velocity.x = 0;
			}

		}

		label.Text = $"{wallInFront}";

		KinematicCollision2D collision = MoveAndCollide(velocity * delta, false, true, false);

		if (collision != null)
		{
			if (collision.Collider is RigidBody2D)
			{
				WhiteBlock rigidBody = collision.Collider as WhiteBlock;

				if (rigidBody.AppliedForce.Abs() == Vector2.Zero ||
					rigidBody.AppliedForce.Abs() < (inertia * rigidBody.AppliedForce.Normalized()))
				{
					rigidBody.ApplyCentralImpulse((velocity/maxSpeed) * (inertia));
				}

				if (onFloor && world.Gravity == World.GravityState.ON)
				{
					PlayerStateMachine statemachine = GetNode<PlayerStateMachine>("PlayerStateMachine");
					if (statemachine.currentState != statemachine.currentState.pushing && velocity.x != 0) 
					{
						statemachine.SetState(statemachine.GetNode<State>("State/Pushing"));
					}
				}
				else if (onObject)
				{
					velocity = velocity.Slide(collision.Normal);
				}
			}
			else
			{
				if (world.Gravity == World.GravityState.ON)
				{
					velocity = velocity.Slide(collision.Normal);

				}
			}

			if (world.Gravity == World.GravityState.OFF)				
			{
				var reflect = collision.Remainder.Bounce(collision.Normal);
				velocity = velocity.Bounce(collision.Normal);
				MoveAndCollide(reflect);
			}
		}
	}   

	public void setPlayerDirection()
	{
		Vector2 dir;
		if (velocity.x != 0)
		{
			dir = velocity.Normalized();
		}
		else if (inputVector.x != 0)
		{
			dir = inputVector.Normalized();
		}
		else
		{
			dir = playerDirection.Normalized();
		}
		
		if (dir.Dot(Vector2.Right) > 0)
		{
			playerDirection = new Vector2(Vector2.Right);
		}
		else
		{
			playerDirection = new Vector2(Vector2.Left);
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
