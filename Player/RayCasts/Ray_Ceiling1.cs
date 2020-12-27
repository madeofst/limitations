using Godot;
using System;

public class Ray_Ceiling1 : RayCasts
{
    public override void checkCollisions()
    {
        if (isCollidingWithTileMap())
        {
            player.onCeiling = true;
        }
    }
}
