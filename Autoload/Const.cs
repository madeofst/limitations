using Godot;
using System;

namespace Limitations
{
    public abstract class Const : Node
    {
        public World world { get; private set; }
        public Player player { get; private set; }
        public PlayerStateMachine playerStateMachine { get; private set; }
        public State state { get; private set; }
        public AnimationPlayer animationPlayer { get; private set; }

        public override void _Ready()
        {
            world = GetNode<World>("/root/World");
            player = world.GetNode<Player>("Player");
            playerStateMachine = player.GetNode<PlayerStateMachine>("PlayerStateMachine");
            //state = playerStateMachine.GetNode<State>("State");
            animationPlayer = player.GetNode<AnimationPlayer>("AnimationPlayer");
        }
    }
}