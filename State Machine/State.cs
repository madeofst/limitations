using Godot;
using System;

namespace Limitations
{

    public abstract class State : Node, IState
    {
        public abstract void enterState();
        public abstract State getReplacement();
        public abstract void exitState();
    }
}