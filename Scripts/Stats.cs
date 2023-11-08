using Godot;
using System;

public partial class Stats : Node
{
	private const int _initialMaxHealth = 2;
	private int _health = _initialMaxHealth;
	
	[Export]
	public int MaxHealth { get; set; } = _initialMaxHealth;

	[Export]
	public int Health {
		get { return _health; } 
		set {
			_health = value;
			if (_health <= 0)
				EmitSignal("HealthEmpty");
		}
	}

	[Signal]
	public delegate void HealthEmptyEventHandler();
	
}
