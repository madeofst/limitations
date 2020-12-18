using Godot;
using System;

public class WorldStateMachine : Node2D
{
    private World parent;
    private Player player;
    private float GlobalGravity;
    public float globalGravity
    {
        get
        {
            return (float)Physics2DServer.AreaGetParam(GetViewport().FindWorld2d().Space, Physics2DServer.AreaParameter.Gravity);
        }
        set
        {
            Physics2DServer.AreaSetParam(GetViewport().FindWorld2d().Space, Physics2DServer.AreaParameter.Gravity, value); //TODO:need to check on value provided
        }
    }

    private float GlobalLinearDamping;
    public float globalLinearDamping
    {
        get
        {
            return (float)Physics2DServer.AreaGetParam(GetViewport().FindWorld2d().Space, Physics2DServer.AreaParameter.LinearDamp);
        }
        set
        {
            Physics2DServer.AreaSetParam(GetViewport().FindWorld2d().Space, Physics2DServer.AreaParameter.LinearDamp, value); //TODO:need to check on value provided
        }
    }


    public enum GravityState
    {
        ON,
        OFF
    }
    private GravityState gravity = GravityState.OFF;
    
    public override void _Ready()
    {
        parent = GetParent<World>();
        player = GetNode<Player>("../Player");
        CallDeferred("ToggleGravity");
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_gravity"))
        {
            ToggleGravity();
        }

        if (@event.IsActionPressed("ui_reset"))
        {
            PackedScene playerScene = (PackedScene)ResourceLoader.Load("res://Player/Player.tscn");
            player.QueueFree();
            Player newPlayer = (Player)playerScene.Instance();
            parent.AddChild(newPlayer);
            PlayerStateMachine sm = newPlayer.GetNode<PlayerStateMachine>("PlayerStateMachine");
            sm.Parent = sm.GetParent<Player>();
        }
    }

    private void ToggleGravity()
    {
        Player player = GetNode<Player>("/root/World/Player");
        if(gravity == GravityState.ON)
        {
            gravity = GravityState.OFF;
            globalGravity = 0;
            globalLinearDamping = 0;

            player.FRICTION = 0;
            player.ACCELERATION = 0;
            player.JUMPSTRENGTH = 0;
        }
        else
        {
            gravity = GravityState.ON;
            globalGravity = 1500;
            globalLinearDamping = 0.1f;

            player.FRICTION = 1500;
            player.ACCELERATION = 1000;
            player.JUMPSTRENGTH = -575;
            
        }
        player.GRAVITYSTRENGTH = player.PlayerGravityMultiplier * (int)globalGravity;
        //GD.Print($"Player GravityStrength = {player.GRAVITYSTRENGTH}");
    }

}
