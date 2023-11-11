using Godot;
using Godot.Collections;
using System;

public partial class SoftCollision : Area2D
{
	public bool IsColliding(out Array<Area2D> areas) {
		areas = GetOverlappingAreas();
		return areas.Count > 0;
	}

	public Vector2 GetPushVector() {
		var pushVector = Vector2.Zero;

		if (IsColliding(out var areas)) {
			var firstArea = areas[0];
			pushVector = firstArea.GlobalPosition.DirectionTo(GlobalPosition);
		}

		return pushVector;
	}
}
