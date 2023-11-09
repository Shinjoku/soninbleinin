using Godot;
using System;

public partial class PlayerDetectionZone : Area2D
{
	public Player Player = null;

	public bool CanSeePlayer() => Player != null;

	public void _OnBodyEntered(Node2D body)
	{
		Player = body as Player;
	}

	public void _OnBodyExited(Node2D body)
	{
		Player = null;
	}
}
