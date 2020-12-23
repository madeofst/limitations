using Godot;
using System;

public class World : Node2D
{
	public enum GravityState
	{
		ON,
		OFF
	}
	public GravityState Gravity { get; set; } = GravityState.ON;

	private float GlobalGravity;
	public float globalGravityValue
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

}
