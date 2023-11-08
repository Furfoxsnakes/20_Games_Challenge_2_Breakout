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
		// TODO: FIX: Paddle freaks out when the mouse position is close to the center of the object
		var direction = Position.DirectionTo(GetGlobalMousePosition());
		direction.Y = 0;
		MoveAndCollide(direction * MoveSpeed);
	}
}
