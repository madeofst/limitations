using Godot;
using Limitations;
using System;

public class Jumping : State
{
    private Player Parent;

    public Jumping(Player parent)
    {
        Name = "Jumping";
        Parent = parent;
    }

    public override State getReplacement()
    {
        if (Parent.grounded)  //TODO: possible signal up
        {
            return new Idle(Parent);
        }
        else if (Parent.velocity.y >= 0)  //TODO: possible signal up
        {
            return new Falling(Parent);
        }
        else return this;
    }

    public override void enterState()
    {
        if (Parent.inputVector.x > 0){
            Parent.animationPlayer.Play("RunRight"); //FIXME: change to jump animation
        }
        else if (Parent.inputVector.x < 0){
            Parent.animationPlayer.Play("RunLeft"); //FIXME: change to jump animation
        }
    }

    public override void exitState()
    {
    }

}
