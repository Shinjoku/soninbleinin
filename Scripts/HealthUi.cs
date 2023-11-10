using System;
using Godot;

public partial class HealthUi : Control
{
	private TextureRect _heartsRect = null;
	private Stats _playerStats = null;

	private int _maxHearts = 4;
	public int MaxHearts
	{
		get => _maxHearts;
		set
		{
			_maxHearts = Math.Max(value, 1);
			if (_heartsRect != null)
				_heartsRect.Size = Vector2.Right * value * 15;
		}
	}

	private int _hearts = 4;
	public int Hearts
	{
		get => _hearts;
		set
		{
			_hearts = Math.Clamp(value, 0, MaxHearts);
			if (_heartsRect != null)
				_heartsRect.Size = Vector2.Right * value * 15;
		}
	}

	public override void _Ready()
	{
		_heartsRect = GetNode<TextureRect>("HeartUiFull");
		_playerStats = GetNode<Stats>("/root/PlayerStats");

		_maxHearts = _playerStats.MaxHealth;
		_hearts = _playerStats.Health;
		_playerStats.HealthChanged += (newHearts) => Hearts = newHearts;
	}
}
