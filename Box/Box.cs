using Godot;
using System;

public class Box : RigidBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //GravityRegion gravityRegion = GetNode<GravityRegion>("../GravityRegion");
        //gravityRegion.Connect("body_entered", this ,nameof(onGravityRegionEnter));
    }

    public void onGravityRegionEnter()
    {
        GD.Print("Entered gravity region.");
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
