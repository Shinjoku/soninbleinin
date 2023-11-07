using Godot;
using System;

public partial class Stats : Node
{
	private const int _initialMaxHealth = 2;
	
	[Export]
	public int MaxHealth { get; set; } = _initialMaxHealth;

	[Export]
	public int Health { get; set; } = _initialMaxHealth;

	[Signal]
	public delegate void HealthEmptyEventHandler();

    public override void _Process(double delta)
    {
        if (Health <= 0)
			EmitSignal(nameof(HealthEmptyEventHandler));
    }
	
	public void SetHealth(int value)
	{
		Health = value;
	}
}
