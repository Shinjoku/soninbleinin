using System;
using Godot;

public partial class Stats : Node
{
	private const int _initialMaxHealth = 2;
	private int _health = _initialMaxHealth;
	private int _maxHealth = _initialMaxHealth;

	[Export]
	public int MaxHealth
	{
		get => _maxHealth;
		set
		{
			_maxHealth = value;
			Health = Math.Min(Health, _maxHealth);
			EmitSignal("MaxHealthChanged", _maxHealth);
		}
	}

	[Export]
	public int Health
	{
		get { return _health; }
		set
		{
			_health = value;
			EmitSignal("HealthChanged", _health);
			if (_health <= 0)
				EmitSignal("HealthEmpty");
		}
	}

	[Signal]
	public delegate void HealthEmptyEventHandler();

	[Signal]
	public delegate void HealthChangedEventHandler(int newHearts);

	[Signal]
	public delegate void MaxHealthChangedEventHandler(int newHearts);

	public override void _Ready()
	{
		Health = MaxHealth;
	}
}
