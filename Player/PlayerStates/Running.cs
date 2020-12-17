using Godot;
using Limitations;
using System;

public class Running : State
{
    private Player Parent;
    
    public Running(Player parent)
    {
        Name = "Running";
        Parent = parent;
    }

    public override State getReplacement()
    {
        if (!Parent.IsOnFloor())
        {
            if(Parent.velocity.y <0) return new Jumping(Parent); //TODO: possible signal up
            else if (Parent.velocity.y >= 0) return new Falling(Parent); //TODO: possible signal up
            else return this;
        }
        else if (Parent.velocity.x < 0.01 && Parent.velocity.x > -0.01) //TODO: possible signal up
        {
            return new Idle(Parent);
        }
        else return this;
    }

    public override void enterState()
    {
        if (Parent.inputVector.x > 0){
            Parent.animationPlayer.Play("RunRight"); //TODO: possible signal up
        }
        else if(Parent.inputVector.x < 0){
            Parent.animationPlayer.Play("RunLeft"); //TODO: possible signal up
        }
    }

    public override void exitState()
    {
    }

}
