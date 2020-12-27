using Godot;
using System;

public class Ray_Ground1 : RayCasts
{   
    public override void checkCollisions()
    {
        if (isCollidingWithTileMap())
        {
            player.grounded = true;
            player.onFloor = true;
        }
        else if (isCollidingWithRigidBody())
        {
            player.grounded = true;
            player.onObject = true;
        }
    }
}
