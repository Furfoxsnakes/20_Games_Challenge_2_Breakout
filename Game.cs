using Godot;
using System;

public partial class Game : Node
{
    [Export] public Ball Ball;
    [Export] public Paddle Paddle;
    [Export] public GameState State = GameState.WaitingForLaunch;
    
    public override void _Ready()
    {
        // Input.MouseMode = Input.MouseModeEnum.Hidden;
    }

    public override void _PhysicsProcess(double delta)
    {
        switch (State)
        {
            case GameState.Playing:
                HandlePlayingState();
                break;
            case GameState.WaitingForLaunch:
                HandleWaitingForLaunchState();
                break;
            case GameState.GameOver:
                HandleGameOverState();
                break;
        }
    }

    private void _on_pit_body_entered(Node2D body)
    {
        if (body is Ball ball)
            State = GameState.WaitingForLaunch;
    }

    private void HandlePlayingState()
    {
        Ball.DoMovement();
    }
    
    private void HandleWaitingForLaunchState()
    {
        Ball.StickToPaddle(Paddle);
        
        if (!Input.IsActionPressed("ui_accept")) return;
        
        Ball.Launch();
        State = GameState.Playing;
    }

    public void HandleGameOverState()
    {
        
    }

    

    public enum GameState
    {
        Playing,
        GameOver,
        WaitingForLaunch
    }
}