using Godot;
using System;

public partial class Hurtbox : Area2D
{
	[Export]
	private bool ShowHitEffect = true;

	private PackedScene HitEffectScene = ResourceLoader.Load<PackedScene>("res://Scenes/HitEffect.tscn");

	public void _OnAreaEntered(Area2D body)
	{
		if (!ShowHitEffect)
			return;

		var effect = HitEffectScene.Instantiate<Effect>();
		effect.GlobalPosition = GlobalPosition;
		GetTree().CurrentScene.AddChild(effect);
	}
}
