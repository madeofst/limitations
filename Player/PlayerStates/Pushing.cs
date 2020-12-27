using Godot;
using Limitations;
using System;

public class Pushing : State
{   
    public Pushing()
    {
        Name = "Pushing";
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

    public override State getReplacement()
    {
        State state = base.getReplacement();

        if (state == null)
        {
            if (player.velocity.x != 0)
            {
                if (player.onLeftWall || player.onRightWall)
                {
                    state = pushing;
                }
                else
                {
                    state = running;
                }
            }
            else
            {
                state = idle;
            }
        }

        return state;
    }

}
