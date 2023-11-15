using Godot;
using System;

public partial class GameCamera : Camera2D
{
	private Marker2D _topLeft;
	private Marker2D _bottomRight;

	public override void _Ready()
	{
		_topLeft = GetNode<Marker2D>("Limits/TopLeft");
		_bottomRight = GetNode<Marker2D>("Limits/BottomRight");

		LimitLeft = (int)_topLeft.Position.X;
		LimitRight = (int)_bottomRight.Position.X;
		LimitTop = (int)_topLeft.Position.Y;
		LimitBottom = (int)_bottomRight.Position.Y;
	}
}
