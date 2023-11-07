using Godot;
using System;

public partial class Bat : CharacterBody2D
{

	private Vector2 _knockback = Vector2.Zero;

	public override void _PhysicsProcess(double delta) {
		_knockback = _knockback.MoveToward(Vector2.Zero, 100 * (float) delta);
		Velocity = _knockback;
		MoveAndSlide();
	}

	private void _OnHurtboxAreaEntered(Area2D area)
	{
		_knockback = Vector2.Right * 90;
		// QueueFree();
	}
}
