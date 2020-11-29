using Godot;
using System;

public class Player : KinematicBody2D
{
    public int FRICTION = 1500;
    public int GRAVITYSTRENGTH = 1000;

    public int ACCELERATION = 1000;
    public int MAXSPEED = 200;
    public int TERMINALVELOCITY = 500; 
    public int JUMPSTRENGTH = -375; //TODO: Look at using properties for these

    private Vector2 baselineGravity;
    public Vector2 inputVector; //TODO: Look at using properties for these
    public Vector2 horizontalDirection;
    public Vector2 velocity;
    public AnimationPlayer animationPlayer;

    public override void _Ready()
    { 
        inputVector = new Vector2(Vector2.Zero);
        horizontalDirection = new Vector2(Vector2.Right);
        baselineGravity = new Vector2(0,TERMINALVELOCITY);
        velocity = new Vector2(baselineGravity);
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public Vector2 detectDirectionalInput()
    {
        inputVector.x = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
        return inputVector;
    }

    public void applyGravity(float delta)
    {
        velocity.y = velocity.MoveToward(baselineGravity, delta * GRAVITYSTRENGTH).y;
    }

    public void updatePlayerPosition(float delta)
    {
        if(inputVector.x != 0)
        {
            horizontalDirection = inputVector;
            velocity.x = velocity.MoveToward(inputVector * MAXSPEED, delta * ACCELERATION).x;
        }
        else
        {
            velocity.x = velocity.MoveToward(Vector2.Zero, delta * FRICTION).x;
        }
        velocity = MoveAndSlide(velocity,Vector2.Up);
    }
}
