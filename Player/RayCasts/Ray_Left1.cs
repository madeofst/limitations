using Godot;
using System;

public class Ray_Left1 : RayCasts
{
    public override void checkCollisions()
    {
        if (isCollidingWithTileMap() || isCollidingWithRigidBody())
        {
            player.onLeftWall = true;
        }
    }
}
