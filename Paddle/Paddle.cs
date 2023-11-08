using Godot;
using System;

public partial class Paddle : StaticBody2D
{
	[Export] private float MoveSpeed = 1;
	private float _yPos;

	public override void _Ready()
	{
		_yPos = Position.Y;
	}

	public override void _PhysicsProcess(double delta)
	{
		// Position = new Vector2(Position.X, _yPos);
		HandleMovement();
	}

	private void HandleMovement()
	{
		var direction = Position.DirectionTo(GetGlobalMousePosition());
		direction.Y = 0;
		MoveAndCollide(direction * MoveSpeed);
	}
}
