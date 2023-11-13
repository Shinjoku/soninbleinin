using Godot;
using System;
using System.Collections.Generic;

enum State
{
	Knockback,
	Idle,
	Wander,
	Chase
}

public partial class Bat : CharacterBody2D
{
	[Export]
	public float InvincibilityTime = 0.3f;
	private PackedScene EnemyDeathEffectScene = ResourceLoader.Load<PackedScene>("res://Scenes/EnemyDeathEffect.tscn");
	private Vector2 _knockback = Vector2.Zero;
	private Stats _stats;
	private State _state = State.Idle;
	private PlayerDetectionZone _playerDetectionZone;
	private AnimatedSprite2D _sprite;
	private Hurtbox _hurtbox = null;
	private SoftCollision _softCollision;
	private WanderController _wanderController;
	private AnimationPlayer _blinkAnimationPlayer;

	private readonly List<State> _idleStates = new()
	{
		State.Idle,
		State.Wander
	};

	[Export]
	public int Acceleration = 50;
	[Export]
	public int MaxSpeed = 50;
	[Export]
	public int Friction = 100;

	public override void _Ready()
	{
		_stats = GetNode<Stats>(nameof(Stats));
		_playerDetectionZone = GetNode<PlayerDetectionZone>(nameof(PlayerDetectionZone));
		_sprite = GetNode<AnimatedSprite2D>("AnimatedSprite");
		_hurtbox = GetNode<Hurtbox>(nameof(Hurtbox));
		_softCollision = GetNode<SoftCollision>(nameof(SoftCollision));
		_wanderController = GetNode<WanderController>(nameof(WanderController));
		_blinkAnimationPlayer = GetNode<AnimationPlayer>("BlinkAnimationPlayer");
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_state == State.Knockback)
			KnockbackState(delta);
		else if (_state == State.Idle)
			IdleState(delta);
		else if (_state == State.Wander)
			WanderState(delta);
		else if (_state == State.Chase)
			ChaseState(delta);

		if (_softCollision.IsColliding(out _))
			Velocity += _softCollision.GetPushVector() * _softCollision.PushForce;

		MoveAndCollide(Velocity * (float)delta);
	}

	private void Stop(double delta) =>
		Velocity = Velocity.MoveToward(Vector2.Zero, Friction * (float)delta);

	private State PickRandomState() => _idleStates[new Random().Next(0, _idleStates.Count)];

	private void KnockbackState(double delta)
	{
		_knockback = _knockback.MoveToward(Vector2.Zero, Friction * (float)delta);
		Velocity = _knockback;

		if (_knockback == Vector2.Zero)
			_state = State.Idle;
	}

	private void ChoosePacificState()
	{
		if (_wanderController.GetTimeLeft() != 0) return;
		_state = PickRandomState();
		_wanderController.StartWanderTimer(new Random().Next(1, 3));
	}

	private void IdleState(double delta)
	{
		DetectPlayer();
		Stop(delta);
		ChoosePacificState();
	}

	private void WanderState(double delta)
	{
		DetectPlayer();
		ChoosePacificState();
		MoveTo(_wanderController.TargetPosition, delta);
	}

	private void ChaseState(double delta)
	{
		var player = _playerDetectionZone.Player;

		if (player == null)
		{
			_state = State.Idle;
			Stop(delta);
			return;
		}

		MoveTo(player.GlobalPosition, delta);
	}

	private void FlipSprite(Vector2 direction) => _sprite.FlipH = direction.X < 0;

	private Vector2 MoveTo(Vector2 targetPosition, double delta)
	{
		var direction = GlobalPosition.DirectionTo(targetPosition);
		Velocity = Velocity.MoveToward(direction * MaxSpeed, Acceleration * (float)delta);
		FlipSprite(direction);
		return direction;
	}

	private void DetectPlayer()
	{
		if (_playerDetectionZone.CanSeePlayer())
			_state = State.Chase;
	}

	private void _OnHurtboxAreaEntered(Area2D area)
	{
		// if (_hurtbox.Invincible) return;
		
		if (area is SwordHitbox swordHitbox)
		{
			_knockback = swordHitbox.KnockbackVector * swordHitbox.KnockbackForce;
			_stats.Health -= swordHitbox.Damage;
			_state = State.Knockback;
			_hurtbox.CreateHitEffect();
			_hurtbox.StartInvincibility(InvincibilityTime);
		}
	}

	private void Die()
	{
		var deathEffect = EnemyDeathEffectScene.Instantiate<Effect>();
		deathEffect.Position = Position;
		GetParent().AddChild(deathEffect);
	}

	public void _OnHealthEmpty()
	{
		Die();
		QueueFree();
	}

	public void _OnBatHurtboxInvincibilityStarted()
	{
		_blinkAnimationPlayer.Play("Start");
	}

	public void _OnBatHurtboxInvincibilityEnded()
	{
		_blinkAnimationPlayer.Play("RESET");
	}
}
