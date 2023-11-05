using Godot;

public partial class Grass : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void Die() {
		var GrassEffectScene = GD.Load<PackedScene>("res://Scenes/GrassEffect.tscn");
		var grassEffect = GrassEffectScene.Instantiate<GrassEffect>();
		grassEffect.Position = Position;
		
		var grassesNode = GetParent();
		grassesNode.AddChild(grassEffect);
	}

	public void _OnHurtboxAreaEntered(Area2D body) {
		Die();
		QueueFree();
	}
}
