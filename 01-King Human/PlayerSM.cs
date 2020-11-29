using Godot;
using System;

public class PlayerSM : StateMachine
{
    private Jump jumping;
    private enum Jump
    {
        FULL,
        REDUCED,
        NONE,
    }

    public override void _Ready()
    {
        CallDeferred("setState",State.idle);
    }

    public override void _Input(InputEvent @event)
    {
        jumping = detectJump(@event);
        if (jumping == Jump.FULL) parent.velocity.y = (float)parent.JUMPSTRENGTH;
        else if (jumping == Jump.REDUCED) parent.velocity.y -= (float)(parent.JUMPSTRENGTH/3);
    }

    private Jump detectJump(InputEvent @event)
    {
        if(state == State.idle | state == State.running)
        {
            if (@event.IsActionPressed("ui_jump") & parent.IsOnFloor()) return Jump.FULL;
            return Jump.NONE;
        }
        else if(state == State.jumping)
        {
            if (@event.IsActionReleased("ui_jump")) return Jump.REDUCED;
            else return Jump.NONE;
        }
        else 
        {
            return Jump.NONE;
        }
    }
    
    public override void stateLogic(float delta)
    {
        Vector2 inputVector = parent.detectDirectionalInput();
        parent.applyGravity(delta);
        parent.updatePlayerPosition(inputVector, delta);
        parent.updateAnimation(inputVector,delta);
    } 

    public override State getTransition(float delta)
    {
/*         switch (state)
        {
            case States.idle:
                if (!parent.IsOnFloor())
                {
                    if (parent.velocity.y < 0)
                    {
                        return States.jumping;
                    }
                }
                break;
        } */

        return state;
    }

    public override void enterState(State newState, State oldState)
    {
        return;
    }

    public override void exitState(State oldState, State newState)
    {
        return;
    }

}
