using Godot;
using System;

public class Player : KinematicBody2D
{
    public int FRICTION;
    public int GRAVITYSTRENGTH;
    public int ACCELERATION;
    public int MAXSPEED = 225;
    public int TERMINALVELOCITY = 500; 
    public int JUMPSTRENGTH; //TODO: Look at using properties for these
    public int INERTIA = 150;

    private Vector2 baselineGravity;
    public Vector2 inputVector; //TODO: Look at using properties for these
    public Vector2 playerDirection;
    public Vector2 velocity;
    public AnimationPlayer animationPlayer;
    private RayCast2D directionalRay;

    public bool grounded { get; set; }
    public bool onLeftWall { get; private set; }
    public bool onRightWall { get; set; }
    public bool onCeiling { get; set; }

    public int PlayerGravityMultiplier 
    { 
        get; 
        internal set; 
    }

    public override void _Ready()
    { 
        inputVector = new Vector2(Vector2.Zero);
        playerDirection = new Vector2(Vector2.Right);
        baselineGravity = new Vector2(0,TERMINALVELOCITY); 
        velocity = new Vector2(baselineGravity);
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        directionalRay = GetNode<RayCast2D>("RayCastVertical/RayCast2D");

        PlayerGravityMultiplier = 1;

        //GravityRegion gravityRegion = GetNode<GravityRegion>("../GravityRegion");
        //gravityRegion.Connect("area_entered", this ,nameof(onGravityRegionEnter));

    }

/*     public void onGravityRegionEnter()
    {
        GD.Print("Entered gravity region.");
    }

    public void onGravityRegionExit()
    {
        GD.Print("Left gravity region.");
    } */

    public void detectDirectionalInput()
    {
        inputVector.y = Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up");
        inputVector.x = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left"); 
    }

    public void applyGravity(float delta)
    {
        if (!grounded) 
        {
            velocity.y += delta * GRAVITYSTRENGTH;
        }
    }

    public void updateHorizontalPlayerPosition(float delta)
    {
        Vector2 horizontalVelocity = new Vector2(velocity.x,0);
        if (onLeftWall && velocity.x < 0)
        {
            velocity.x = 0;
        }
        else if (onRightWall && velocity.x > 0)
        {
            velocity.x = 0;
        }
        else if (inputVector.x != 0)
        {
            velocity.x = horizontalVelocity.MoveToward(inputVector * MAXSPEED, delta * ACCELERATION).x;
        }
        else
        {
            velocity.x = horizontalVelocity.MoveToward(Vector2.Zero, FRICTION * delta).x;
        }

        GD.Print(velocity);
        KinematicCollision2D collision = MoveAndCollide(velocity * delta, false, true, false);
        if (collision != null)
        {
            velocity = velocity.Slide(collision.Normal);
        }
        
        
        //if (velocity != Vector2.Zero) GD.Print($"velocity = {velocity}");
        //velocity = MoveAndSlide(velocity, Vector2.Up, false, 4, (float)Mathf.Pi/4, false);
    }

    public void checkAllCollisions()
    {
        CheckSurfaceCollisions();
        CheckStaticBodyCollisions();
    }

    public void CheckSurfaceCollisions()
    {
        grounded = false;
        onLeftWall = false;
        onRightWall = false;
        onCeiling = false;
        
        Node2D rayCasts = GetNode<Node2D>("RayCasts");
        for (int i=0; i<=rayCasts.GetChildCount()-1; i++)
        {
            RayCast2D ray = rayCasts.GetChild<RayCast2D>(i);
            if (ray.IsColliding())
            {
                Vector2 normalizedRayVector = ray.CastTo.Normalized();
                if (Vector2.Down == normalizedRayVector)
                {
                    grounded = true;
                }
                else if (Vector2.Left == normalizedRayVector)
                {
                    onLeftWall = true;
                }
                else if ( Vector2.Right == normalizedRayVector)
                {
                    onRightWall = true;
                }
                else if (Vector2.Up == normalizedRayVector)
                {
                    onCeiling = true;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
    
    private void CheckStaticBodyCollisions()
    {
        for (int i=0; i<=GetSlideCount()-1; i++) //TODO: getslidecount won't work any more
        {
            KinematicCollision2D collision = GetSlideCollision(i);
            if (collision.Collider.IsClass("RigidBody2D"))
            {
                RigidBody2D c = collision.Collider as RigidBody2D;
                c.ApplyCentralImpulse(-collision.Normal * INERTIA);
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
}
