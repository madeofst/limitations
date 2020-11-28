using Godot;
using System;

public class Player : KinematicBody2D
{
    private int FRICTION = 1500;
    private int GRAVITYSTRENGTH = 1000;

    private int ACCELERATION = 1000;
    private int MAXSPEED = 200; 
    private double JUMPSTRENGTH = -375;
    private int TERMINALVELOCITY = 500;

    private Vector2 horizontalDirection;
    private Vector2 baselineGravity;
    private Vector2 velocity;
    private AnimationPlayer animationPlayer;
    private Jump jumping;

    enum Jump
    {
        FULL,
        REDUCED,
        NONE,
    }

    public override void _Ready()
    { 
        horizontalDirection = new Vector2(Vector2.Right);
        baselineGravity = new Vector2(0,TERMINALVELOCITY);
        velocity = new Vector2(baselineGravity);
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public override void _PhysicsProcess(float delta)
    {   
        Vector2 inputVector = detectDirectionalInput();
        updatePlayerPosition(inputVector, jumping, delta);
        updateAnimation(inputVector, jumping, delta);
    }

    public override void _Input(InputEvent @event)
    {
        jumping = detectJump(@event);
        //update vertical position
        if (jumping == Jump.FULL) velocity.y = (float)JUMPSTRENGTH;
        else if (jumping == Jump.REDUCED) velocity.y -= (float)(JUMPSTRENGTH/3);
    }

    private Jump detectJump(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_jump") & IsOnFloor()) return Jump.FULL;
        else if (@event.IsActionReleased("ui_jump")) return Jump.REDUCED;
        else return Jump.NONE;
    }

    private Vector2 detectDirectionalInput()
    {
        Vector2 inputVector = new Vector2(Vector2.Zero);
        inputVector.x = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
        return inputVector;
    }

    private void updatePlayerPosition(Vector2 inputVector, Jump jumping, float delta)
    {
        velocity.y = velocity.MoveToward(baselineGravity, delta * GRAVITYSTRENGTH).y;
        //update horizontal position
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

    private void updateAnimation(Vector2 inputVector, Jump jumping, float delta)
    {
        if (jumping != Jump.NONE)
        {

        }
        else if(inputVector.x != 0)
        {
            if (inputVector.x > 0){
                animationPlayer.Play("RunRight");
            }
            else if(inputVector.x < 0){
                animationPlayer.Play("RunLeft");
            }
        }
        else
        {
            if (horizontalDirection.x > 0){
                animationPlayer.Play("IdleRight");
            }
            else if (horizontalDirection.x < 0){
                animationPlayer.Play("IdleLeft");
            }
        }
    }
}
