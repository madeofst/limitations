using Godot;
using System;

public class BlockRayCastSprite : Sprite
{
    private RayCast2D rayCast;

    public override void _Ready()
	{
        rayCast = GetNode<RayCast2D>("..");
    }

    public override void _PhysicsProcess(float delta)
    {
        Rotation = rayCast.Rotation;
    }
}
