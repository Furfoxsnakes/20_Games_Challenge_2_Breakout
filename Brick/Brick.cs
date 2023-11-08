using Godot;
using System;

public partial class Brick : StaticBody2D
{
	[Export] public int Points;
	private Game _game;

	public void Init(Game game)
	{
		_game = game;
	}

	public void Hit()
	{
		_game.Bricks.Remove(this);
		QueueFree();
	}
}
