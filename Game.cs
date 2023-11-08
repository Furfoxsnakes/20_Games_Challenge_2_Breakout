using Godot;
using System;
using System.Collections.Generic;

public partial class Game : Node
{
    [ExportCategory("Game Objects")]
    [Export] public Ball Ball;
    [Export] public Paddle Paddle;
    [Export] public GameState State = GameState.WaitingForLaunch;

    [ExportCategory("UI")]
    [Export] private CanvasLayer _gameOverScreen;
    [Export] private Container _heartsContainer;
    [Export] private Label _pressSpaceLabel;
    
    public List<Brick> Bricks = new List<Brick>();
    public int Score = 0;
    
    public override void _Ready()
    {
        foreach (var row in GetNode("Bricks").GetChildren())
        {
            foreach (var child in row.GetChildren())
            {
                if (child is Brick brick)
                {
                    brick.Init(this);
                    Bricks.Add(brick);
                }
            }
        }
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
        {
            State = LoseHeart();
            Ball.ResetCurrentSpeed();
        }
    }

    private void _on_game_restart_pressed()
    {
        RestartGame();
    }

    private void HandlePlayingState()
    {
        GD.Print(Bricks.Count);
        _pressSpaceLabel.Visible = false;
        Ball.DoMovement();
    }
    
    private void HandleWaitingForLaunchState()
    {
        _pressSpaceLabel.Visible = true;
        
        Ball.StickToPaddle(Paddle);
        
        if (!Input.IsActionPressed("ui_accept")) return;
        
        Ball.Launch();
        _pressSpaceLabel.Visible = false;
        State = GameState.Playing;
    }

    public void HandleGameOverState()
    {
        _gameOverScreen.Visible = true;
    }

    public void BrickHit(Brick brick)
    {
        
    }

    private GameState LoseHeart()
    {
        if (_heartsContainer.GetChildCount() == 0) return GameState.GameOver;
        
        _heartsContainer.GetChild(0).Free();
        
        GD.Print(_heartsContainer.GetChildCount());

        if (_heartsContainer.GetChildCount() == 0)
            return GameState.GameOver;

        return GameState.WaitingForLaunch;
    }

    private void RestartGame()
    {
        GetTree().ReloadCurrentScene();
    }

    public enum GameState
    {
        Playing,
        GameOver,
        WaitingForLaunch
    }
}