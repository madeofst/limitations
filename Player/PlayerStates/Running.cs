using Godot;
using Limitations;
using System;

public class Running : State
{   
    public Running()
    {
        Name = "Running";
    }

    public override void enterState()
    {
        if (player.inputVector.x > 0)
        {
            animationPlayer.Play("RunRight");
        }
        else if(player.inputVector.x < 0)
        {
            animationPlayer.Play("RunLeft");
        }
    }

    public override void exitState()
    {
    }

}
