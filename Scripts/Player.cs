using System;
using Godot;

enum AnimationTreeParameters
{
    BlendPosition
}

enum PlayerState
{
    Move,
    Roll,
    Attack
}

public partial class Player : CharacterBody2D
{
    public const float Speed = 80.0f;
    private AnimationTree _animationTree = null;
    private AnimationNodeStateMachinePlayback _animationState = null;
    private AnimationPlayer _animationPlayer = null;
    private PlayerState _playerState = PlayerState.Move;

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    public override void _Ready()
    {
        _animationTree = GetNode<AnimationTree>(nameof(AnimationTree));
        _animationState = (AnimationNodeStateMachinePlayback)_animationTree.Get("parameters/playback");
        _animationPlayer = GetNode<AnimationPlayer>(nameof(AnimationPlayer));

        _animationTree.Active = true;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_playerState == PlayerState.Move)
            MoveState();
        else if (_playerState == PlayerState.Attack)
            AttackState();
    }

    public Vector2 Walk(Vector2 direction)
    {
        SetAnimationTree("Idle", direction);
        SetAnimationTree("Run", direction);
        SetAnimationTree("Attack", direction);
        _animationState.Travel("Run");
        return direction * Speed;
    }

    public Vector2 Stop()
    {
        _animationState.Travel("Idle");
        return Vector2.Zero;
    }

    public void AttackAnimationFinished()
    {
        _playerState = PlayerState.Move;
    }

    private void SetAnimationTree(string animation, Vector2 vector, AnimationTreeParameters param = AnimationTreeParameters.BlendPosition) =>
        _animationTree.Set($"parameters/{animation}/{GetParameterString(param)}", vector);

    private string GetParameterString(AnimationTreeParameters param) => param switch
    {
        AnimationTreeParameters.BlendPosition => "blend_position",
        _ => throw new NotImplementedException()
    };

    private void MoveState()
    {
        Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

        Velocity = direction != Vector2.Zero ?
            Walk(direction) : Stop();

        MoveAndSlide();

        if (Input.IsActionJustPressed("attack"))
            _playerState = PlayerState.Attack;
    }

    private void AttackState()
    {
        _animationState.Travel("Attack");
    }

    // Commented code:
    // Add the gravity.
    // if (!IsOnFloor())
    // 	velocity.Y += gravity * (float)delta;

    // Handle Jump.
    // if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
    // 	velocity.Y = JumpVelocity;
}
