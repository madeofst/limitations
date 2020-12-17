using Godot;
using Limitations;
using System;

public class Idle : State
{
    private Player Parent;

    public Idle(Player parent)
    {
        Name = "Idle";
        Parent = parent;
    }

    public override State getReplacement()
    {
        if(!Parent.IsOnFloor())
        {
            if(Parent.velocity.y <0) return new Jumping(Parent); //TODO: possible signal up
            else if (Parent.velocity.y > 0) return new Falling(Parent); //TODO: possible signal up
            else return this;
        }
        else if(Parent.velocity.x !=0)
        {
            return new Running(Parent);
        }
        else return this;
    }

    public override void enterState()
    {
        if (Parent.playerDirection.x > 0){ 
            Parent.animationPlayer.Play("IdleRight"); //TODO: possible signal up
        }
        else if (Parent.playerDirection.x < 0){
            Parent.animationPlayer.Play("IdleLeft"); //TODO: possible signal up
        }
    }

    public override void exitState()
    {

    }

}
