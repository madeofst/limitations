using Godot;
using System;

public class WorldSM : Node2D
{
    private World parent;
    
    public enum GravityState
    {
        ON,
        OFF
    }
    public GravityState gravity = GravityState.ON;
    
    public override void _Ready()
    {
        parent = GetParent<World>();
        GD.Print(gravity);
    }

    public override void _Input(InputEvent @event)
    {
        ToggleGravity(@event);
    }

    public override void _PhysicsProcess(float delta)
    {
        Player player = GetNode<Player>("/root/World/Player");
        if (gravity == GravityState.ON)
        {
            player.GRAVITYSTRENGTH = 1000;
            player.FRICTION = 1500;
            player.ACCELERATION = 1000;
            player.MAXSPEED = 200;
            player.TERMINALVELOCITY = 500; 
            player.JUMPSTRENGTH = -375;
        }
        else if (gravity == GravityState.OFF)
        {
            player.GRAVITYSTRENGTH = 0;
            player.FRICTION = 0;
            player.ACCELERATION = 0;
            player.MAXSPEED = 200;
            player.TERMINALVELOCITY = 0; 
            player.JUMPSTRENGTH = 0;
        }
    }

    private void ToggleGravity(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_gravity"))
		{
			if(gravity == GravityState.ON)
			{
				gravity = GravityState.OFF;
			}
			else
			{
				gravity = GravityState.ON;
			}
 			GD.Print(gravity);
		}
    }

}
