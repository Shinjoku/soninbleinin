using Godot;

public partial class Grass : Node2D
{
	private PackedScene GrassEffectScene = ResourceLoader.Load<PackedScene>("res://Scenes/GrassEffect.tscn");

	private void Die()
	{
		var grassEffect = GrassEffectScene.Instantiate<Effect>();
		grassEffect.Position = Position;

		var grassesNode = GetParent();
		grassesNode.AddChild(grassEffect);
	}

	public void _OnHurtboxAreaEntered(Area2D body)
	{
		Die();
		QueueFree();
	}
}
