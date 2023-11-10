using Godot;
using System;

public partial class Stats : Node
{
	private const int _initialMaxHealth = 2;
	private int _health = _initialMaxHealth;

	[Export]
	public int MaxHealth { get; set; } = _initialMaxHealth;

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
}
