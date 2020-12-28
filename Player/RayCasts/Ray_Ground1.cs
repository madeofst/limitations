using Godot;
using System;

public class Ray_Ground1 : RayCasts
{   
    public override void checkCollisions()
    {
        if (isCollidingWithTileMap())
        {
            player.onFloor = true;
        }
        else if (isCollidingWithRigidBody())
        {
            player.onObject = true;
        }

        if (player.onFloor || player.onObject) player.grounded = true;
    }
}
