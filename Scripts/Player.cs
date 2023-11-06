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
    public const float WalkSpeed = 60.0f;
    public const float RollSpeed = 120.0f;
    private AnimationTree _animationTree = null;
    private AnimationNodeStateMachinePlayback _animationState = null;
    private PlayerState _playerState = PlayerState.Move;

    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    public override void _Ready()
    {
        _animationTree = GetNode<AnimationTree>(nameof(AnimationTree));
        _animationState = (AnimationNodeStateMachinePlayback)_animationTree.Get("parameters/playback");

        _animationTree.Active = true;
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

        if (_playerState == PlayerState.Move)
            MoveState(direction);
        else if (_playerState == PlayerState.Roll)
            RollState(direction);
        else if (_playerState == PlayerState.Attack)
            AttackState();
    }

    public Vector2 Walk(Vector2 direction)
    {
        SetAnimationTree("Idle", direction);
        SetAnimationTree("Run", direction);
        SetAnimationTree("Attack", direction);
        SetAnimationTree("Roll", direction);
        _animationState.Travel("Run");
        return direction * WalkSpeed;
    }

    public Vector2 Roll(Vector2 direction) {
        _animationState.Travel("Roll");
        return direction * RollSpeed;
    }

    public Vector2 Stop()
    {
        _animationState.Travel("Idle");
        return Vector2.Zero;
    }

    public void ResetPlayerState()
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

    private void MoveState(Vector2 direction)
    {
        Velocity = direction != Vector2.Zero ?
            Walk(direction) : Stop();

        MoveAndSlide();

        if (Input.IsActionJustPressed("attack"))
            _playerState = PlayerState.Attack;
        else if (Input.IsActionJustPressed("roll"))
            _playerState = PlayerState.Roll;
    }

    private void RollState(Vector2 direction) {
        Velocity = Roll(direction);
        MoveAndSlide();
        _animationState.Travel("Roll");
    }

    private void AttackState()
    {
        _animationState.Travel("Attack");
    }

}
