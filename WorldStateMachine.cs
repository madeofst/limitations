using Godot;
using System;

public class WorldStateMachine : Node2D
{
    private World parent;
    
    public enum GravityState
    {
        ON,
        OFF
    }
    private GravityState gravity = GravityState.OFF;
    
    public override void _Ready()
    {
        parent = GetParent<World>();
        GD.Print(gravity);
        ToggleGravity();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_gravity"))
        {
            ToggleGravity();
        }
    }

    private void ToggleGravity()
    {
        Player player = GetNode<Player>("/root/World/Player");
        if(gravity == GravityState.ON)
        {
            gravity = GravityState.OFF;
            player.GRAVITYSTRENGTH = 0;
            player.FRICTION = 0;
            player.ACCELERATION = 0;
            //player.MAXSPEED = 225;
            player.JUMPSTRENGTH = 0;
        }
        else
        {
            gravity = GravityState.ON;
            player.GRAVITYSTRENGTH = 1800;
            player.FRICTION = 1500;
            player.ACCELERATION = 1000;
            //player.MAXSPEED = 225;
            player.JUMPSTRENGTH = -575;
        }
        GD.Print(gravity);
    }

}
