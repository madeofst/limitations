using Godot;
using System;

public class WorldStateMachine : Node2D
{
	private World world;
	private Player player;
	private Node2D objects;
	private WhiteBlock block;
	
	public override void _Ready()
	{
		world = GetParent<World>();
		player = GetNode<Player>("../Player");
		objects = GetNode<Node2D>("../Objects");
		if (objects.GetNode<WhiteBlock>("WhiteBlock") != null)
		{
			block = objects.GetNode<WhiteBlock>("WhiteBlock"); //TODO: This needs to refer to all white blocks
		}
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

			if (block != null)
			{
				PackedScene boxScene = (PackedScene)ResourceLoader.Load("res://Box/WhiteBlock.tscn");
				objects.RemoveChild(objects.GetNode<RigidBody2D>("WhiteBlock"));
				block = (WhiteBlock)boxScene.Instance();
				objects.AddChild(block);
				block.Position = new Vector2(140,112);
			}

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
		updateWorldGravity();
		updatePlayerGravity();
		updateObjectGravity();
	}

	private void updateWorldGravity()
	{
		if(world.Gravity == World.GravityState.ON)
		{
			world.globalGravityValue = 1500;
			world.globalLinearDamping = 0.1f;
		}
		else if (world.Gravity == World.GravityState.OFF)
		{
			world.globalGravityValue = 0;
			world.globalLinearDamping = 0;
		}
		else
		{
			throw new NotImplementedException();
		}
	}
	private void updatePlayerGravity()
	{
		if(world.Gravity == World.GravityState.ON)
		{
			player.friction = 1500;
			player.accleration = 1000;
			player.jumpStrength = -475;
		}
		else if (world.Gravity == World.GravityState.OFF)
		{
			player.friction = 0;
			player.accleration = 0;
			player.jumpStrength = 0;
		}
		player.gravityStrength = player.playerGravityMultiplier * (int)world.globalGravityValue;
	}

	private void updateObjectGravity()
	{

		for (int i = 0; i<=objects.GetChildCount()-1; i++)
		{
			WhiteBlock obj = objects.GetChild<WhiteBlock>(i);
			if(world.Gravity == World.GravityState.ON)
			{
				obj.blockState = WhiteBlock.BlockState.falling;
			}
			else if (world.Gravity == World.GravityState.OFF)
			{
				obj.blockState = WhiteBlock.BlockState.floating;
			}
			else
			{
				throw new NotImplementedException();
			}
		}
	}
}
