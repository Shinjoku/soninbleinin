using System;
using Godot;

enum AnimationTreeParameters
{
	BlendPosition
}

public partial class Player : CharacterBody2D
{
	public const float Speed = 80.0f;
	public const float JumpVelocity = -400.0f;
	private AnimationTree _animationTree = null;
	private AnimationNodeStateMachinePlayback _animationState = null;


	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _Ready()
	{
		_animationTree = GetNode<AnimationTree>(nameof(AnimationTree));
		_animationState = (AnimationNodeStateMachinePlayback)_animationTree.Get("parameters/playback");
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

		Velocity = direction != Vector2.Zero ?
			Walk(direction) : Stop(velocity);

		MoveAndSlide();
	}

	public Vector2 Walk(Vector2 direction)
	{
		SetAnimationTree("Idle", direction);
		SetAnimationTree("Run", direction);
		_animationState.Travel("Run");
		return direction * Speed;
	}

	public Vector2 Stop(Vector2 velocity)
	{
		_animationState.Travel("Idle");
		return velocity.MoveToward(Vector2.Zero, Speed);
	}

	private void SetAnimationTree(string animation, Vector2 vector, AnimationTreeParameters param = AnimationTreeParameters.BlendPosition) =>
		_animationTree.Set($"parameters/{animation}/{GetParameterString(param)}", vector);

	private string GetParameterString(AnimationTreeParameters param) => param switch
	{
		AnimationTreeParameters.BlendPosition => "blend_position",
		_ => throw new NotImplementedException()
	};

	// Commented code:
	// Add the gravity.
	// if (!IsOnFloor())
	// 	velocity.Y += gravity * (float)delta;

	// Handle Jump.
	// if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
	// 	velocity.Y = JumpVelocity;
}
