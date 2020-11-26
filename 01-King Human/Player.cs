using Godot;
using System;

public class Player : KinematicBody2D
{
    private int ACCELERATION = 850;
    private int MAXSPEED = 275;
    private int FRICTION = 1200;
    private int GRAVITYSTRENGTH = 1250;
    private int JUMPSTRENGTH = -250;
    private int JUMPACCELERATION = 5000;

    private Vector2 baselineGravity;
    private Vector2 horizontalDirection = new Vector2(Vector2.Zero);
    private Vector2 velocity = new Vector2(Vector2.Zero);
    private AnimationPlayer animationPlayer;
    private bool jumping;

    public override void _Ready()
    { 
        animationPlayer = GetNode<AnimationPlayer>("PlayerAnimationPlayer");
        horizontalDirection = Vector2.Right;
        baselineGravity = new Vector2(0,GRAVITYSTRENGTH);
        velocity = baselineGravity;
    }

    public override void _PhysicsProcess(float delta)
    {
        Vector2 inputVector = new Vector2(Vector2.Zero);
        //inputVector.x = 1;
        inputVector.x = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
        if (Input.IsActionJustPressed("ui_jump") & IsOnFloor())
        //if (IsOnFloor())
        {
            jumping = true;
        }
        else if (Input.IsActionJustReleased("ui_jump") | IsOnFloor() | velocity.y <= JUMPSTRENGTH + (delta * GRAVITYSTRENGTH))
        {
            jumping = false;
        }
        if (jumping) {velocity.y = velocity.MoveToward(Vector2.Down * JUMPSTRENGTH, delta * JUMPACCELERATION).y;}
        velocity.y = velocity.MoveToward(baselineGravity, delta * GRAVITYSTRENGTH).y;

        if(inputVector.x != 0)
        {
            horizontalDirection = inputVector;
            if (inputVector.x > 0){
                animationPlayer.Play("RunRight");
            }
            else if(inputVector.x < 0){
                animationPlayer.Play("RunLeft");
            }
            velocity.x = velocity.MoveToward(inputVector * MAXSPEED, delta * ACCELERATION).x;
        }
        else
        {
            if (horizontalDirection.x > 0){
                animationPlayer.Play("IdleRight");
            }
            else if (horizontalDirection.x < 0){
                animationPlayer.Play("IdleLeft");
            }
            velocity.x = velocity.MoveToward(Vector2.Zero, delta * FRICTION).x;
        }

        velocity = MoveAndSlide(velocity,Vector2.Up);

        //Node2D world = GetParent<Node2D>();
        GD.Print("");
    }

}
