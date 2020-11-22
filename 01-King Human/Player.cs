using Godot;
using System;

public class Player : KinematicBody2D
{
    private int ACCELERATION = 800;
    public int MAXSPEED = 300;

    private Vector2 velocity = new Vector2(Vector2.Zero);
    private AnimationPlayer aPlayer;

    public override void _PhysicsProcess(float delta)
    {
        Vector2 inputVector = new Vector2(Vector2.Zero);
        aPlayer = GetNode<AnimationPlayer>("PlayerAnimationPlayer");

        inputVector.x = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
        velocity = velocity.MoveToward(inputVector * MAXSPEED, delta * ACCELERATION);
        if(inputVector.x != 0)
        {
            aPlayer.Play("IdleRight");
        }
        else
        {
            
        }
        velocity = MoveAndSlide(velocity);
        //GD.Print(velocity.x);

        //Node2D world = GetParent<Node2D>();
        //GD.Print(world.Name);
    }

}
