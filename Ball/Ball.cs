using Godot;
using System;

public partial class Ball : CharacterBody2D
{
	[Export] private int MoveSpeed = 50;
	public Vector2 Movement;

	public override void _Ready()
	{
		Movement = new Vector2(1, 1);
		Velocity = Movement * MoveSpeed;
	}

	public override void _PhysicsProcess(double delta)
	{
		HandleCollision(MoveAndSlide());
	}

	private void HandleCollision(bool isColliding)
	{
		if (!isColliding) return;

		// GD.Print(GetSlideCollision(0).GetTravel());

		var collider = GetSlideCollision(0).GetCollider();
		
		if (collider is Brick brick)
			brick.Hit();
		
		var travel = GetSlideCollision(0).GetTravel().Normalized();

		if (IsOnFloor())
			Velocity = new Vector2(travel.X, -travel.Y) * MoveSpeed;
		
		if (IsOnCeiling())
			Velocity = new Vector2(travel.X, -travel.Y) * MoveSpeed;
		
		if (IsOnWall())
		{
			Velocity = new Vector2(-travel.X, travel.Y) * MoveSpeed;
		}
	}
}
