using Godot;
using System;

enum State
{
	Knockback,
	Idle,
	Wander,
	Chase
}

public partial class Bat : CharacterBody2D
{
	private PackedScene EnemyDeathEffectScene = ResourceLoader.Load<PackedScene>("res://Scenes/EnemyDeathEffect.tscn");
	private Vector2 _knockback = Vector2.Zero;
	private Stats _stats;
	private State _state = State.Idle;
	private PlayerDetectionZone _playerDetectionZone;
	private AnimatedSprite2D _sprite;
	private Hurtbox _hurtbox = null;
	private SoftCollision _softCollision;

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
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_state == State.Knockback)
			KnockbackState(delta);
		else if (_state == State.Idle)
			IdleState(delta);
		else if (_state == State.Wander)
			WanderState();
		else if (_state == State.Chase)
			ChaseState(delta);

		if (_softCollision.IsColliding())
		{
			Velocity += _softCollision.GetPushVector() * (float)delta * 400;
		}

		MoveAndCollide(Velocity * (float)delta);
	}

	private void Stop(double delta)
	{
		Velocity = Velocity.MoveToward(Vector2.Zero, Friction * (float)delta);
	}

	private void KnockbackState(double delta)
	{
		_knockback = _knockback.MoveToward(Vector2.Zero, Friction * (float)delta);
		Velocity = _knockback;

		if (_knockback == Vector2.Zero)
			_state = State.Idle;
	}

	private void IdleState(double delta)
	{
		DetectPlayer();
		Stop(delta);
	}

	private void WanderState()
	{ }

	private void ChaseState(double delta)
	{
		var player = _playerDetectionZone.Player;

		if (player == null)
		{
			_state = State.Idle;
			Stop(delta);
			return;
		}

		var direction = (player.GlobalPosition - GlobalPosition).Normalized();
		Velocity = Velocity.MoveToward(direction * MaxSpeed, Acceleration * (float)delta);
		_sprite.FlipH = direction.X < 0;
	}

	private void DetectPlayer()
	{
		if (_playerDetectionZone.CanSeePlayer())
			_state = State.Chase;
	}

	private void _OnHurtboxAreaEntered(Area2D area)
	{
		if (area is SwordHitbox swordHitbox)
		{
			_knockback = swordHitbox.KnockbackVector * swordHitbox.KnockbackForce;
			_stats.Health -= swordHitbox.Damage;
			_state = State.Knockback;
			_hurtbox.CreateHitEffect();
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
}
