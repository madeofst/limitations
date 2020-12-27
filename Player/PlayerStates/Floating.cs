using Godot;
using Limitations;
using System;

public class Floating : State
{
    public Floating()
    {
        Name = "Floating";
    }

    public override void enterState()
    {
        if (player.inputVector.x > 0)
        {
            animationPlayer.Play("IdleRight");  //FIXME: change to floating
        }
        else if (player.inputVector.x < 0)
        {
            animationPlayer.Play("IdleLeft");  //FIXME: change to floating
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
            state = falling;
        }

        return state;
    }

}