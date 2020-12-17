using Godot;
using Limitations;
using System;

public class Falling : State
{
    private Player Parent;

    public Falling(Player parent)
    {
        Name = "Falling";
        Parent = parent;
    }
    
    public override State getReplacement()
    {
        if (Parent.IsOnFloor())  //TODO: possible signal up
        {
            return new Idle(Parent);
        }
        else if (Parent.velocity.y < 0)  //TODO: possible signal up
        {
            return new Jumping(Parent);
        }
        else return this;
    }

    public override void enterState()
    {
        if (Parent.inputVector.x > 0){
            Parent.animationPlayer.Play("IdleRight");  //FIXME: change to fall animation
        }
        else if (Parent.inputVector.x < 0){
            Parent.animationPlayer.Play("IdleLeft");  //FIXME: change to fall animation
        }
    }

    public override void exitState()
    {
    }


}