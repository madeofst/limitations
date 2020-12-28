using Godot;
using System;

public class WhiteBlock : RigidBody2D
{
    public enum BlockState
    {
        falling,
        grounded,
        floating
    }

    public BlockState blockState { get; set; } = BlockState.grounded;

    private World world { get; set; }
    private RayCast2D ray { get; set; }
    
    public override void _Ready()
    {
        world = GetNode<World>("/root/World");
        ray = GetNode<RayCast2D>("RayCast2D");
    }

    private float prevAngularVelocity;
    private Vector2 prevLinearVelocity;
    private Vector2 prevPosition;
    private Vector2 prevprevPosition;

    public override void _IntegrateForces(Physics2DDirectBodyState state)
    {
    }

    public override void _PhysicsProcess(float delta)
    {
        //GD.Print(AppliedForce.Abs().x);


        ray.Rotation = -Rotation;
        
        if (!ray.IsColliding() && blockState == BlockState.grounded)
        {
            blockState = BlockState.falling;
        }

        if (Position == prevPosition)
        {
            if (prevprevPosition != prevPosition) 
            {
                blockState = BlockState.grounded;
            }
            prevprevPosition = prevPosition;
        }
        else
        {
            //TODO: An extra check here to cover edge cases where it doesn't switch back.
        }

        prevPosition = Position;

        //GD.Print(blockState);

        if (blockState == BlockState.falling)
        {
            //Mass = 100;
            PhysicsMaterialOverride.Bounce = 0f;
            Mode = RigidBody2D.ModeEnum.Rigid;
        }
        else if (blockState == BlockState.grounded)
        {
            //Mass = 2;
            PhysicsMaterialOverride.Bounce = 0f;
            Mode = RigidBody2D.ModeEnum.Character;
        }
        else if (blockState == BlockState.floating)
        {
            //Mass = 2;
            PhysicsMaterialOverride.Bounce = 1;
            Mode = RigidBody2D.ModeEnum.Rigid;
        }
        else
        {
            blockState = BlockState.grounded;
            throw new NotImplementedException();
        }

    }

}
