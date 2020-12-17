using Godot;
using System;

namespace Limitations
{
    public interface IState
    {
        void enterState();
        State getReplacement();
        void exitState();
    }
}