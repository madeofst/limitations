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

    public override State getReplacement()
    {
        State state = base.getReplacement();

        if (state == null)
        {
            if (player.grounded)
            {
                if (player.velocity.x == 0)
                {
                    state = idle;
                }
                else
                {
                    state = running;
                }
            }
            else
            {
                state = falling;
            }
        }

        return state;
    }

}
