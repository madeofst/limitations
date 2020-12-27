using Godot;
using Limitations;
using System;

public class Jumping : State
{
    public Jumping()
    {
        Name = "Jumping";
    }

    public override void enterState()
    {
        if (player.inputVector.x > 0)
        {
            animationPlayer.Play("RunRight"); //FIXME: change to jump animation
        }
        else if (player.inputVector.x < 0)
        {
            animationPlayer.Play("RunLeft"); //FIXME: change to jump animation
        }
    }

    public override void exitState()
    {
    }

    public override State getReplacement()
    {
        State state = base.getReplacement();

        if (state == null)
        {
            if (player.velocity.y < 0) 
            {
                return jumping;
            }
            else
            {
                return falling;
            }
        }

        return state;
    }

}
