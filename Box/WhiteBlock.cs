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

    public BlockState blockState { get; set; } = BlockState.falling;

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
        ray.Rotation = -Rotation;
        
        if (!ray.IsColliding() && blockState == BlockState.grounded)
        {
            blockState = BlockState.falling;
        }

        if (ray.GetCollider() is TileMap && Position == prevPosition)
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

        if (blockState == BlockState.falling)
        {
            PhysicsMaterialOverride.Bounce = 0f;
            Mode = RigidBody2D.ModeEnum.Rigid;
        }
        else if (blockState == BlockState.grounded)
        {
            PhysicsMaterialOverride.Bounce = 0f;
            Mode = RigidBody2D.ModeEnum.Character;
        }
        else if (blockState == BlockState.floating)
        {
            PhysicsMaterialOverride.Bounce = 1;
            Mode = RigidBody2D.ModeEnum.Rigid;
        }
        else
        {
            blockState = BlockState.grounded;
            throw new NotImplementedException();
        }

        GetNode<RichTextLabel>("RichTextLabel").Text = blockState.ToString();

    }

}
