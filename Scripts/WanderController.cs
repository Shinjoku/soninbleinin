using System;
using Godot;

public partial class WanderController : Node2D
{
	public Vector2 StartPosition;
	public Vector2 TargetPosition;
	private Random _random;

	private Timer _timer;

	[Export]
	public int WanderRange = 32;

	public override void _Ready()
	{
		_timer = GetNode<Timer>(nameof(Timer));
		StartPosition = GlobalPosition;
		TargetPosition = GlobalPosition;
		_random = new Random();
	}

	private int GetRandomRange() => _random.Next(-WanderRange, WanderRange);

	public void UpdateTargetPosition()
	{
		var rnd = new Random();
		var targetVector = new Vector2(GetRandomRange(), GetRandomRange());
		TargetPosition = StartPosition + targetVector;
	}

	public double GetTimeLeft() => _timer.TimeLeft;

	public void StartWanderTimer(double duration) => _timer.Start(duration);


	public void _OnTimerTimeout() => UpdateTargetPosition();
}
