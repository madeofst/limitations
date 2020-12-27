using Godot;
using System;

public class Ray_Right1 : RayCasts
{
    public override void checkCollisions()
    {
        if (isCollidingWithTileMap() || isCollidingWithRigidBody())
        {
            player.onRightWall = true;
        }
    }
}
