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
    [Export]
    public float WalkSpeed = 60.0f;
    [Export]
    public float RollSpeed = 120.0f;
    private AnimationTree _animationTree = null;
    private AnimationNodeStateMachinePlayback _animationState = null;
    private PlayerState _playerState = PlayerState.Move;
    private SwordHitbox _swordHitbox = null;
    private Hurtbox _hurtbox = null;
    private Stats _playerStats = null;
    private Sprite2D _animatedSprite = null;
    private PackedScene _playerHurtSound = ResourceLoader.Load<PackedScene>("res://Scenes/PlayerHurtSound.tscn");
    private AnimationPlayer _blinkAnimationPlayer = null;

    public override void _Ready()
    {
        _animationTree = GetNode<AnimationTree>(nameof(AnimationTree));
        _animationState = (AnimationNodeStateMachinePlayback)_animationTree.Get("parameters/playback");
        _swordHitbox = GetNode<SwordHitbox>("HitboxPivot/SwordHitbox");
        _playerStats = GetNode<Stats>("/root/PlayerStats");
        _hurtbox = GetNode<Hurtbox>(nameof(Hurtbox));
        _animatedSprite = GetNode<Sprite2D>("Sprite");
        _blinkAnimationPlayer = GetNode<AnimationPlayer>("BlinkAnimationPlayer");

        _animationTree.Active = true;
        _playerStats.HealthEmpty += QueueFree;
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        if (Input.IsActionJustPressed("invisibility"))
            _animatedSprite.Visible = !_animatedSprite.Visible;

        if (_playerState == PlayerState.Move)
            MoveState(direction);
        else if (_playerState == PlayerState.Roll)
            RollState(direction);
        else if (_playerState == PlayerState.Attack)
            AttackState();
    }

    private void SetAnimationTree(string animation, Vector2 vector, AnimationTreeParameters param = AnimationTreeParameters.BlendPosition)
    {
        _animationTree.Set($"parameters/{animation}/{GetParameterString(param)}", vector);
    }

    private string GetParameterString(AnimationTreeParameters param) => param switch
    {
        AnimationTreeParameters.BlendPosition => "blend_position",
        _ => throw new NotImplementedException()
    };

    #region Actions

    public Vector2 Walk(Vector2 direction)
    {
        SetAnimationTree("Idle", direction);
        SetAnimationTree("Run", direction);
        SetAnimationTree("Attack", direction);
        SetAnimationTree("Roll", direction);

        _swordHitbox.KnockbackVector = direction;

        _animationState.Travel("Run");
        return direction * WalkSpeed;
    }

    public Vector2 Roll(Vector2 direction)
    {
        _animationState.Travel("Roll");
        return direction * RollSpeed;
    }

    public Vector2 Stop()
    {
        _animationState.Travel("Idle");
        return Vector2.Zero;
    }

    #endregion

    #region States

    public void ResetPlayerState() => _playerState = PlayerState.Move;

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

    private void RollState(Vector2 direction)
    {
        Velocity = Roll(direction);
        MoveAndSlide();
        _animationState.Travel("Roll");
    }

    private void AttackState() => _animationState.Travel("Attack");

    #endregion

    public void _OnHurtboxAreaEntered(Area2D area)
    {
        if (_hurtbox.Invincible) return;

        _playerStats.Health -= 1;
        _hurtbox.StartInvincibility(0.5);
        _hurtbox.CreateHitEffect();
        
        var hitSound = _playerHurtSound.Instantiate<PlayerHurtSound>();
        GetParent().AddChild(hitSound);
    }

    public void _OnPlayerHurtboxInvincibilityStarted()
    {
        _blinkAnimationPlayer.Play("Start");
    }

    public void _OnPlayerHurtboxInvincibilityEnded()
    {
        _blinkAnimationPlayer.Play("RESET");
    }
}
