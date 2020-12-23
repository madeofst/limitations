using Godot;
using System;

public class WorldStateMachine : Node2D
{
    private World world;
    private Player player;
    private WhiteBlock block;
    
    public override void _Ready()
    {
        world = GetParent<World>();
        player = GetNode<Player>("../Player");
        block = GetNode<WhiteBlock>("../WhiteBlock");
        setGravityState(world.Gravity);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_gravity"))
        {
            ToggleGravityState();
        }

        if (@event.IsActionPressed("ui_reset"))
        {
            PackedScene playerScene = (PackedScene)ResourceLoader.Load("res://Player/Player.tscn");
            world.RemoveChild(player);
            player = (Player)playerScene.Instance();
            world.AddChild(player);

            PackedScene boxScene = (PackedScene)ResourceLoader.Load("res://Box/WhiteBlock.tscn");
            world.RemoveChild(world.GetNode<RigidBody2D>("WhiteBlock"));
            block = (WhiteBlock)boxScene.Instance();
            world.AddChild(block);
            block.Position = new Vector2(140,112);

            setGravityState(world.Gravity);
        }
    }

    private void ToggleGravityState()
    {
        if(world.Gravity == World.GravityState.ON)
        {
            setGravityState(World.GravityState.OFF);
        }
        else
        {
            setGravityState(World.GravityState.ON);
        }
    }

    private void setGravityState(World.GravityState gravity)
    {
        world.Gravity = gravity;
        if(world.Gravity == World.GravityState.ON)
        {
            world.globalGravityValue = 1500;
            world.globalLinearDamping = 0.1f;

            player.friction = 1500;
            player.accleration = 1000;
            player.jumpStrength = -475;

            block.blockState = WhiteBlock.BlockState.falling;
        }
        else if (world.Gravity == World.GravityState.OFF)
        {
            world.globalGravityValue = 0;
            world.globalLinearDamping = 0;

            player.friction = 0;
            player.accleration = 0;
            player.jumpStrength = 0;

            block.blockState = WhiteBlock.BlockState.floating;
        }
        else
        {
            throw new NotImplementedException();
        }
        player.gravityStrength = player.playerGravityMultiplier * (int)world.globalGravityValue;
    }
}
