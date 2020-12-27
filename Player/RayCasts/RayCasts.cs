using Godot;
using System;

public class RayCasts : RayCast2D
{
    public World world { get; set; }
    public Player player { get; set; }

    public override void _Ready()
    {
        world = GetNode<World>("/root/World");
        player = world.GetNode<Player>("Player");
    }
    
    public virtual void checkCollisions()
    {
    }

    public bool isCollidingWithTileMap()
    {
        if (GetCollider() is TileMap)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool isCollidingWithRigidBody()
    {
        if (GetCollider() is RigidBody2D)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
