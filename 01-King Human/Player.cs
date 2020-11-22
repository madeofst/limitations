using Godot;
using System;

public class Player : KinematicBody2D
{
    private int ACCELERATION = 850;
    private int MAXSPEED = 325;
    private int FRICTION = 1200;

    private Vector2 velocity = new Vector2(Vector2.Zero);
    private AnimationPlayer aPlayer;

    public override void _Ready()
    {
        aPlayer = GetNode<AnimationPlayer>("PlayerAnimationPlayer");
    }

    public override void _PhysicsProcess(float delta)
    {
        Vector2 inputVector = new Vector2(Vector2.Zero);
        inputVector.x = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");

        if(inputVector.x != 0)
        {
            if (inputVector.x > 0){
                aPlayer.Play("RunRight");
            }
            else{
                aPlayer.Play("RunLeft");
            }
            velocity = velocity.MoveToward(inputVector * MAXSPEED, delta * ACCELERATION);
        }
        else
        {
            if (velocity.x > 0){
                aPlayer.Play("IdleRight");
            }
            else if (velocity.x < 0){
                aPlayer.Play("IdleLeft");
            }
            velocity = velocity.MoveToward(Vector2.Zero, delta * FRICTION);
        }
        velocity = MoveAndSlide(velocity);

        //Node2D world = GetParent<Node2D>();
        //GD.Print(world.Name);
    }

}
