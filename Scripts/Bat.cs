using Godot;
using System;

public partial class Bat : CharacterBody2D
{
	private Vector2 _knockback = Vector2.Zero;

	private Stats _stats;

    public override void _Ready()
    {
        _stats = GetNode<Stats>(nameof(Stats));
    }

    public override void _PhysicsProcess(double delta) {
		_knockback = _knockback.MoveToward(Vector2.Zero, 100 * (float) delta);
		Velocity = _knockback;
		MoveAndSlide();
	}

	private void _OnHurtboxAreaEntered(Area2D area)
	{
		if (area is SwordHitbox swordHitbox) 
		{
			_knockback = swordHitbox.KnockbackVector * swordHitbox.KnockbackForce;
			_stats.SetHealth(_stats.Health - swordHitbox.Damage);
		}
	}

	public void _OnHealthEmpty()
	{
		QueueFree();
	}
}
