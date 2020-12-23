using Godot;
using Limitations;
using System;

public class Player : KinematicBody2D
{
	public int friction;
	public int gravityStrength;
	public int accleration;
	public int maxSpeed = 225;
	public int terminalVelocity = 500; 
	public int jumpStrength; //TODO: Look at using properties for these
	public int inertia = 300;
	public float drag = 0.5f;
	public int playerGravityMultiplier { get; internal set; }  = 1;

	public Vector2 inputVector; 
	public Vector2 playerDirection;
	public Vector2 velocity;
	
	public World world { get; private set; }
	//public AnimationPlayer animationPlayer { get; private set; }

	private RayCast2D directionalRay;
	private RigidBody2D rigidBody;

	public bool onFloor { get; private set; }
	public bool onLeftWall { get; private set; }
	public bool onRightWall { get; private set; }
	public bool onCeiling { get; private set; }
	public bool onObject { get; private set; }
	public bool grounded { get; private set; }

	public override void _Ready()
	{ 
		world = GetNode<World>("/root/World");
		directionalRay = GetNode<RayCast2D>("RayCastVertical/RayCast2D");

		inputVector = new Vector2(Vector2.Zero);
		playerDirection = new Vector2(Vector2.Right);
		velocity = new Vector2(0,terminalVelocity);

		//printParameters();
	}

	public void detectDirectionalInput()
	{
		inputVector.y = Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up");
		inputVector.x = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left"); 
	}

	public void CheckSurfaceCollisions()
	{
		grounded = false;
		onObject = false;
		onFloor = false;
		onLeftWall = false;
		onRightWall = false;
		onCeiling = false;
		
		Node2D rayCasts = GetNode<Node2D>("RayCasts");
		for (int i=0; i<=rayCasts.GetChildCount()-1; i++)
		{
			RayCast2D ray = rayCasts.GetChild<RayCast2D>(i);
			if (ray.IsColliding() && (ray.GetCollider() is TileMap))
			{
				if (Vector2.Down == ray.CastTo.Normalized())
				{
					onFloor = true;
					grounded = true;
				}
				else if (Vector2.Left == ray.CastTo.Normalized())
				{
					onLeftWall = true;
				}
				else if (Vector2.Right == ray.CastTo.Normalized())
				{
					onRightWall = true;
				}
				else if (Vector2.Up == ray.CastTo.Normalized())
				{
					onCeiling = true;
				}
				else
				{
					throw new NotImplementedException();
				}
			}
			else if (ray.IsColliding() && (ray.GetCollider() is RigidBody2D))
			{
				if (Vector2.Down == ray.CastTo.Normalized())
				{
					onObject = true;
					grounded = true;
				}
			}
		}
		//printInteractionStatus();
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
			if (world.Gravity == World.GravityState.ON)
			{
				velocity.x = horizontalVelocity.MoveToward(Vector2.Zero, drag * velocity.x * delta).x;
			}
		}

		if (world.Gravity == World.GravityState.ON)
		{
			if (onLeftWall && velocity.x < 0)
			{
				velocity.x = 0;
			}
			else if (onRightWall && velocity.x > 0)
			{
				velocity.x = 0;
			}
		}

		KinematicCollision2D collision = MoveAndCollide(velocity * delta, false, true, false);

		if (collision != null)
		{
			if (collision.Collider.IsClass("RigidBody2D") && grounded)
			{
				rigidBody = collision.Collider as RigidBody2D;
				rigidBody.ApplyCentralImpulse(-collision.Normal * inertia);
			}
			
			if (world.Gravity == World.GravityState.ON)
			{
				velocity = velocity.Slide(collision.Normal);
			}
			else if (world.Gravity == World.GravityState.OFF)
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
