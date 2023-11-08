using Godot;
using System;

public partial class Game : Node
{
    
    
    public override void _Ready()
    {
        // Input.MouseMode = Input.MouseModeEnum.Hidden;
    }
    
    public enum GameState
    {
        Playing,
        GameOver,
        WaitingForLaunch
    }
}