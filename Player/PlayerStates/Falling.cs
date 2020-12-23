using Godot;
using Limitations;
using System;

public class Falling : State
{
    public Falling()
    {
        Name = "Falling";
    }

    public override void enterState()
    {
        if (player.inputVector.x > 0)
        {
            animationPlayer.Play("IdleRight");  //FIXME: change to fall animation
        }
        else if (player.inputVector.x < 0)
        {
            animationPlayer.Play("IdleLeft");  //FIXME: change to fall animation
        }
    }

    public override void exitState()
    {
    }

}