using Godot;
using System;

public partial class Ball : CharacterBody2D
{
	[Export] private float MoveSpeed = 50;
	public Vector2 Movement;

	public override void _Ready()
	{
		Velocity = Vector2.Down * MoveSpeed;
	}

	public override void _PhysicsProcess(double delta)
	{
		HandleCollision(MoveAndSlide());
	}

	public void Launch()
	{
		Velocity = new Vector2(0, -1).Normalized() * MoveSpeed;
	}

	private void HandleCollision(bool isColliding)
	{
		if (!isColliding) return;

		// GD.Print(GetSlideCollision(0).GetTravel());

		var collider = GetSlideCollision(0).GetCollider();
		
		if (collider is Brick brick)
		{
			brick.Hit();
			MoveSpeed *= 1.05f;
		}
		
		var travel = GetSlideCollision(0).GetTravel();

		// add some variance to the ball trajectory depending on where the ball hits the paddle
		// more variance added the farther away from the center of the paddle that the ball hits
		if (collider is Paddle paddle)
			travel.X -= GlobalPosition.DirectionTo(paddle.GlobalPosition).X * 0.15f;

		travel = travel.Normalized();

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
