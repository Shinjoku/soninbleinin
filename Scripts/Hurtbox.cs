using Godot;

public partial class Hurtbox : Area2D
{
	private bool _invincible = false;

	public bool Invincible
	{
		get => _invincible;
		set
		{
			_invincible = value;
			EmitSignal(_invincible ? "InvincibilityStarted" : "InvincibilityEnded");
		}
	}

	private Timer _timer = null;

	[Signal]
	public delegate void InvincibilityStartedEventHandler();

	[Signal]
	public delegate void InvincibilityEndedEventHandler();

	private PackedScene HitEffectScene = ResourceLoader.Load<PackedScene>("res://Scenes/HitEffect.tscn");

	public override void _Ready()
	{
		_timer = GetNode<Timer>(nameof(Timer));
	}

	public void CreateHitEffect()
	{
		var effect = HitEffectScene.Instantiate<Effect>();
		effect.GlobalPosition = GlobalPosition;
		GetTree().CurrentScene.AddChild(effect);
	}

	#region Timer

	public void _OnTimerTimeout() => _invincible = false;

	public void StartInvincibility(double duration)
	{
		_invincible = true;
		_timer.Start(duration);
	}

	#endregion

	#region Event Handlers

	public void _OnHurtboxInvincibilityStarted() => SetDeferred("Monitorable", false);

	public void _OnHurtboxInvincibilityEnded() => SetDeferred("Monitorable", true);

	#endregion
}
