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

    private Vector2 baselineGravity;
    public Vector2 inputVector; //TODO: Look at using properties for these
    public Vector2 playerDirection;
    public Vector2 velocity;
    public AnimationPlayer animationPlayer;
    private RayCast2D rayCast;

    public override void _Ready()
    { 
        inputVector = new Vector2(Vector2.Zero);
        playerDirection = new Vector2(Vector2.Right);
        baselineGravity = new Vector2(0,TERMINALVELOCITY); 
        velocity = new Vector2(baselineGravity);
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        rayCast = GetNode<RayCast2D>("RayCastVertical/RayCast2D");
    }

    public void detectDirectionalInput()
    {
        inputVector.y = Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up");
        inputVector.x = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
    }

    public void applyGravity(float delta)
    {
        velocity.y += delta * GRAVITYSTRENGTH;
    }

    public void updatePlayerPosition(float delta)
    {
        Vector2 horizontalVelocity = new Vector2(velocity.x,0);
        if (inputVector.x != 0)
        {
            velocity.x = horizontalVelocity.MoveToward(inputVector * MAXSPEED, delta * ACCELERATION).x;
        }
        else
        {
            velocity.x = horizontalVelocity.MoveToward(Vector2.Zero, FRICTION * delta).x;
        }
        velocity = MoveAndSlide(velocity, Vector2.Up);
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
        rayCast.Rotation = Mathf.Atan2(playerDirection.y, playerDirection.x);
    }
}
