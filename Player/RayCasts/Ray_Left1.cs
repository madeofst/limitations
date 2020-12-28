using Godot;
using System;

public class Ray_Left1 : RayCasts
{
    public override void checkCollisions()
    {
        if (player.playerDirection.Normalized().Dot(CastTo.Normalized()) == 1)
        {
            if (isCollidingWithTileMap())
            {
                //player.onRightWall = true;
                player.wallInFront = true;
            }
            else if (isCollidingWithRigidBody())
            {
                player.objectInFront = true;
            }
        }
    }
}
