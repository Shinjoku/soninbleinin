using Godot;
using System;

public partial class SoftCollision : Area2D
{
	public bool IsColliding() {
		var areas = GetOverlappingAreas();
		return areas.Count > 0;
	}

	public Vector2 GetPushVector() {
		var areas = GetOverlappingAreas();
		var pushVector = Vector2.Zero;

		if (IsColliding()) {
			var firstArea = areas[0];
			pushVector = firstArea.GlobalPosition.DirectionTo(GlobalPosition);
		}

		return pushVector;
	}
}
