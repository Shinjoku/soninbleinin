using Godot;
using System;

public partial class Hurtbox : Area2D
{
	private PackedScene HitEffectScene = ResourceLoader.Load<PackedScene>("res://Scenes/HitEffect.tscn");

	public void _OnAreaEntered(Area2D body) {
		var effect = HitEffectScene.Instantiate<Effect>();
		effect.GlobalPosition = GlobalPosition;
		GetTree().CurrentScene.AddChild(effect);
	}
}
