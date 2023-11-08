using Godot;
using System;
using System.Collections.Generic;

public partial class Ball : CharacterBody2D
{
	[Export] private AudioStreamPlayer _hitAudio;
	[Export] private float MoveSpeed = 50;
	public Vector2 Movement;

	private float _maxSpeed => MoveSpeed * 5;
	private float _currentSpeed;
	private const float MinAngle = 0.5f;

	public override void _Ready()
	{
		Velocity = Vector2.Down * MoveSpeed;
		_currentSpeed = MoveSpeed;
	}

	public void StickToPaddle(Paddle paddle)
	{
		Position = new Vector2(paddle.Position.X, paddle.Position.Y - 7);
	}

	public void Launch()
	{
		Velocity = new Vector2(0, -1).Normalized() * MoveSpeed;
	}

	public void DoMovement()
	{
		HandleCollision(MoveAndSlide());
	}

	public void ResetCurrentSpeed()
	{
		_currentSpeed = MoveSpeed;
	}

	private void HandleCollision(bool isColliding)
	{
		if (!isColliding) return;

		_hitAudio.Play();

		var collider = GetSlideCollision(0).GetCollider();
		
		if (collider is Brick brick)
		{
			brick.Hit();
			_currentSpeed = MathF.Min(_currentSpeed * 1.05f, _maxSpeed);
		}
		
		var travel = GetSlideCollision(0).GetTravel();

		// add some variance to the ball trajectory depending on where the ball hits the paddle
		// more variance added the farther away from the center of the paddle that the ball hits
		if (collider is Paddle paddle)
			travel.X -= GlobalPosition.DirectionTo(paddle.GlobalPosition).X * 0.15f;

		travel = travel.Normalized();

		if (IsOnFloor())
			Velocity = new Vector2(travel.X, -travel.Y);
		
		if (IsOnCeiling())
			Velocity = new Vector2(travel.X, -travel.Y);
		
		if (IsOnWall())
			Velocity = new Vector2(-travel.X, travel.Y);
		
		//Stretch Goal: Add a minimum angle to the ball so that it doesn't do steep horizontal bounces off the walls
		
		Velocity *= _currentSpeed;
	}
}
