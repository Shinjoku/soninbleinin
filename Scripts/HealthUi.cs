using System;
using Godot;

public partial class HealthUi : Control
{
	private TextureRect _heartsRect = null;
	private TextureRect _emptyHeartsRect = null;
	private Stats _playerStats = null;

	private int _maxHearts = 4;
	public int MaxHearts
	{
		get => _maxHearts;
		set
		{
			_maxHearts = Math.Max(value, 1);
			Hearts = Math.Min(Hearts, MaxHearts);
			if (_emptyHeartsRect != null) {
				Vector2 textureSize = _emptyHeartsRect.Texture.GetSize();
				_emptyHeartsRect.Size = new Vector2(value * textureSize.X, textureSize.Y);
			}
		}
	}

	private int _hearts = 4;
	public int Hearts
	{
		get => _hearts;
		set
		{
			_hearts = Math.Clamp(value, 0, MaxHearts);
			if (_heartsRect != null) {
				Vector2 textureSize = _heartsRect.Texture.GetSize();
				_heartsRect.Size = new Vector2(value * textureSize.X, textureSize.Y);
			}
		}
	}

	public override void _Ready()
	{
		_heartsRect = GetNode<TextureRect>("HeartUiFull");
		_emptyHeartsRect = GetNode<TextureRect>("HeartUiEmpty");
		_playerStats = GetNode<Stats>("/root/PlayerStats");

		MaxHearts = _playerStats.MaxHealth;
		_playerStats.HealthChanged += (newHearts) => Hearts = newHearts;
		_playerStats.MaxHealthChanged += (newHearts) => MaxHearts = newHearts;
	}
}
