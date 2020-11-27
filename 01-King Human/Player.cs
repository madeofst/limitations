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

    enum jump
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
        animationPlayer = GetNode<AnimationPlayer>("PlayerAnimationPlayer");
    }

    public override void _PhysicsProcess(float delta)
    {   
        Vector2 inputVector = detectDirectionalInput();
        jump jumping = detectJump();
        updatePlayerPosition(inputVector, jumping, delta);
        updateAnimation(inputVector, jumping, delta);
    }

    private Vector2 detectDirectionalInput()
    {
        Vector2 inputVector = new Vector2(Vector2.Zero);
        inputVector.x = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
        return inputVector;
    }

    private jump detectJump()
    {
        if (Input.IsActionJustPressed("ui_jump") & IsOnFloor()) return jump.FULL;
        else if (Input.IsActionJustReleased("ui_jump")) return jump.REDUCED;
        else return jump.NONE;
    }

    private void updatePlayerPosition(Vector2 inputVector, jump jumping, float delta)
    {
        //update vertical position
        if (jumping == jump.FULL) velocity.y = (float)JUMPSTRENGTH;
        else if (jumping == jump.REDUCED) velocity.y -= (float)(JUMPSTRENGTH/3);
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

    private void updateAnimation(Vector2 inputVector, jump jumping, float delta)
    {
        if (jumping != jump.NONE)
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
