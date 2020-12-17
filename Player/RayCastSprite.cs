using Godot;
using Limitations;
using System;

public class RayCastSprite : Sprite
{
    private RayCast2D rayCast;

    public override void _Ready()
	{
        rayCast = GetNode<RayCast2D>("../RayCast2D");
    }

    public override void _PhysicsProcess(float delta)
    {
        Rotation = rayCast.Rotation;
    }

}
