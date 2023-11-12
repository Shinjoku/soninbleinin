using Godot;
using System;

public partial class PlayerHurtSound : AudioStreamPlayer
{
	public override void _Ready()
	{
		Finished += QueueFree;
	}
}
