using Godot;
using Limitations;
using System;

public class Idle : State
{
	public Idle()
	{
		Name = "Idle";
	}

	public override void enterState()
	{
		if (player.playerDirection.x > 0)
		{ 
			animationPlayer.Play("IdleRight"); 
		}
		else if (player.playerDirection.x < 0)
		{
			animationPlayer.Play("IdleLeft");
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
                if (player.velocity.x != 0)
                {
                    state = running;
                }
                else
                {
                    state = idle;
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
